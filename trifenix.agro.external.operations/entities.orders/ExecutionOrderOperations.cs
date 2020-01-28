using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.helper;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.model.external;
using trifenix.agro.model.external.output;
using trifenix.agro.search;
using trifenix.agro.search.model;

namespace trifenix.agro.external.operations.entities.orders
{
    public class ExecutionOrderOperations : IExecutionOrderOperations {

        private readonly IExecutionOrderRepository _repo;
        private readonly IApplicationOrderRepository _repoOrders;
        private readonly IUserRepository _repoUsers;
        private readonly INebulizerRepository _repoNebulizers;
        private readonly IProductRepository _repoProducts;
        private readonly ITractorRepository _repoTractors;
        private readonly ICommonDbOperations<ExecutionOrder> _commonDb;
        private readonly IGraphApi _graphApi;
        private readonly string _idSeason;
        private readonly AgroSearch _searchServiceInstance;
        private readonly string entityName = "ExecutionOrder";

        public ExecutionOrderOperations(IExecutionOrderRepository repo, IApplicationOrderRepository repoOrders, IUserRepository repoUsers, INebulizerRepository repoNebulizers, IProductRepository repoProducts, ITractorRepository repoTractors, ICommonDbOperations<ExecutionOrder> commonDb, IGraphApi graphApi, string idSeason, AgroSearch searchServiceInstance) {
            _repo = repo;
            _repoOrders = repoOrders;
            _repoUsers = repoUsers;
            _repoNebulizers = repoNebulizers;
            _repoProducts = repoProducts;
            _repoTractors = repoTractors;
            _commonDb = commonDb;
            _graphApi = graphApi;
            _idSeason = idSeason;
            _searchServiceInstance = searchServiceInstance;
        }

        public async Task<ExtGetContainer<ExecutionOrder>> GetExecutionOrder(string id) {
            var executionOrder = await _repo.GetExecutionOrder(id);
            return OperationHelper.GetElement(executionOrder);
        }

        public async Task<ExtGetContainer<List<ExecutionOrder>>> GetExecutionOrders() {
            var executionOrdersQuery = _repo.GetExecutionOrders();
            var executionOrders = await _commonDb.TolistAsync(executionOrdersQuery);
            return OperationHelper.GetElements(executionOrders);
        }

        private async Task<List<ProductToApply>> GetProductsExecution(string[] idsProduct, double[] quantities) {
            if (idsProduct.Count() != quantities.Count())
                throw new Exception("Los productos no cumplen el criterio.");
            var max = idsProduct.Count();
            var list = new List<ProductToApply>();
            for (int i = 0; i < max; i++){
                var product = await _repoProducts.GetProduct(idsProduct[i]);
                var localQuantity = quantities[i];
                list.Add(new ProductToApply {
                    Product = product,
                    QuantityByHectare = localQuantity
                });
            }
            return list;
        }


        public async Task<ExtPostContainer<string>> SaveNewExecutionOrder(string idOrder, string executionName, string idUserApplicator, string idNebulizer, string[] idsProduct, double[] quantitiesByHectare, string idTractor, string commentary) {
            if (string.IsNullOrWhiteSpace(idOrder)) return OperationHelper.GetPostException<string>(new Exception("Es requerido 'idOrder' para crear una ejecucion."));
            ApplicationOrder order = await _repoOrders.GetApplicationOrder(idOrder);
            if (order == null)
                return OperationHelper.PostNotFoundElementException<string>($"No se encontró la orden de aplicacion con id {idOrder}", idOrder);
            var executions = _repo.GetExecutionOrders().Where(execution => execution.Order.Id.Equals(order.Id));
            if(executions.Count(execution => execution.ClosedStatus == (ClosedStatus)1) > 0)
                return OperationHelper.GetPostException<string>(new Exception($"No se puede crear una nueva ejecucion para la orden {idOrder}, debido a que ya existe una ejecucion terminada exitosamente para esta orden."));
            if (idsProduct == null || !idsProduct.Any())
                return OperationHelper.GetPostException<string>(new Exception("Se requiere al menos un producto.")); 
            UserApplicator userApplicator = null;
            Nebulizer nebulizer = null;
            Tractor tractor = null;
            if (!String.IsNullOrWhiteSpace(idUserApplicator)) {
                userApplicator = await _repoUsers.GetUser(idUserApplicator);
                if (userApplicator == null)
                    return OperationHelper.PostNotFoundElementException<string>($"No se encontró el usuario aplicador con id {idUserApplicator}", idUserApplicator);
            }
            if (!String.IsNullOrWhiteSpace(idNebulizer)) {
                nebulizer = await _repoNebulizers.GetNebulizer(idNebulizer);
                if (nebulizer == null)
                    return OperationHelper.PostNotFoundElementException<string>($"No se encontró la nebulizadora con id {idNebulizer}", idNebulizer);
            }
            if (!String.IsNullOrWhiteSpace(idTractor)) {
                tractor = await _repoTractors.GetTractor(idTractor);
                if (tractor == null)
                    return OperationHelper.PostNotFoundElementException<string>($"No se encontró el tractor con id {idTractor}", idTractor);
            }
            UserApplicator creator = await _graphApi.GetUserFromToken();
            UserActivity userActivity = new UserActivity(DateTime.Now, creator);
            var productApplies = await GetProductsExecution(idsProduct, quantitiesByHectare);
            var createOperation = await OperationHelper.CreateElement(_commonDb, _repo.GetExecutionOrders(),
               async s => await _repo.CreateUpdateExecutionOrder(new ExecutionOrder {
                   Id = s,
                   SeasonId = _idSeason,
                   Name = executionName,
                   Order = order,
                   UserApplicator = userApplicator,
                   Nebulizer = nebulizer,
                   ProductToApply = productApplies,
                   Tractor = tractor,
                   Creator = userActivity
               }),
               s => s.Name.Equals(executionName),
               $"Ya existe ejecucion con nombre: {executionName}");
            _searchServiceInstance.AddEntities(new List<EntitySearch> {
                new EntitySearch {
                    Created = DateTime.Now,
                    EntityName = entityName,
                    Name = executionName,
                    Id = createOperation.IdRelated
                }
            });
            var setStatusOperation = await SetStatus(createOperation.IdRelated, "execution", 0, commentary);
            return new ExtPostContainer<string> {
                IdRelated = setStatusOperation.IdRelated,
                Result = setStatusOperation.IdRelated,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<ExecutionOrder>> SaveEditExecutionOrder(string id, string executionName, string idOrder, string idUserApplicator, string idNebulizer, string[] idsProduct, double[] quantitiesByHectare, string idTractor) {
            ExecutionOrder execution = await _repo.GetExecutionOrder(id);
            if(execution.ExecutionStatus > 0)
                return OperationHelper.GetPostException<ExecutionOrder>(new Exception("Solo se puede modificar la ejecucion cuando esta en planificacion."));
            if (string.IsNullOrWhiteSpace(idOrder)) return OperationHelper.GetPostException<ExecutionOrder>(new Exception("Es requerido 'idOrder'."));
            ApplicationOrder order = await _repoOrders.GetApplicationOrder(idOrder);
            if (order == null)
                return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró la orden de aplicacion con id {idOrder}", idOrder);
            if (idsProduct == null || !idsProduct.Any())
                return OperationHelper.GetPostException<ExecutionOrder>(new Exception("Se requiere al menos un producto.")); UserApplicator userApplicator = null;
            Nebulizer nebulizer = null;
            Tractor tractor = null;
            if (!String.IsNullOrWhiteSpace(idUserApplicator)) {
                userApplicator = await _repoUsers.GetUser(idUserApplicator);
                if (userApplicator == null)
                    return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró el usuario aplicador con id {idUserApplicator}", idUserApplicator);
            }
            if (!String.IsNullOrWhiteSpace(idNebulizer)) {
                nebulizer = await _repoNebulizers.GetNebulizer(idNebulizer);
                if (nebulizer == null)
                    return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró la nebulizadora con id {idNebulizer}", idNebulizer);
            }
            if (!String.IsNullOrWhiteSpace(idTractor)) {
                tractor = await _repoTractors.GetTractor(idTractor);
                if (tractor == null)
                    return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró el tractor con id {idTractor}", idTractor);
            }
            UserApplicator modifier = await _graphApi.GetUserFromToken();
            UserActivity userActivity = new UserActivity(DateTime.Now, modifier);
            var productApplies = await GetProductsExecution(idsProduct, quantitiesByHectare);
            return await OperationHelper.EditElement(_commonDb, _repo.GetExecutionOrders(),
                id,
                execution,
                s => {
                    _searchServiceInstance.AddEntities(new List<EntitySearch> {
                        new EntitySearch{
                            Created = DateTime.Now,
                            EntityName = entityName,
                            Name = executionName,
                            Id = s.Id
                        }
                    });
                    s.Name = executionName;
                    s.Order = order;
                    s.UserApplicator = userApplicator;
                    s.Nebulizer = nebulizer;
                    s.ProductToApply = productApplies;
                    s.Tractor = tractor;
                    s.ModifyBy.Add(userActivity);
                    return s;
                },
                _repo.CreateUpdateExecutionOrder,
                 $"No existe orden de ejecucion con id: {id}",
                s => s.Name.Equals(executionName) && executionName != execution.Name,
               $"Ya existe ejecucion con nombre: {executionName}");
        }

        public async Task<ExtPostContainer<ExecutionOrder>> SetStatus(string idExecutionOrder, string type, int value, string commentary) {
            string error = "";
            if (string.IsNullOrWhiteSpace(idExecutionOrder)) error += "Es requerido 'idExecutionOrder' para identificar la ejecucion a modificar.\n";
            if (string.IsNullOrWhiteSpace(type)) error += "Es requerido 'type' para identificar el tipo de estado a modificar en la ejecucion.\n";
            if (!string.IsNullOrEmpty(error)) return OperationHelper.GetPostException<ExecutionOrder>(new Exception(error));
            ExecutionOrder execution = await _repo.GetExecutionOrder(idExecutionOrder);
            if (execution == null)
                return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró la ejecucion con id {idExecutionOrder}", idExecutionOrder);
            if (execution.ClosedStatus != 0)
                return OperationHelper.GetPostException<ExecutionOrder>(new Exception($"No se puede modificar el estado {type}, ya que se ha cerrado la ejecucion.\n"));
            if (execution.FinishStatus != 0 && !(type.ToLower().Equals("closed") || (type.ToLower().Equals("execution") && value == 3)))
                return OperationHelper.GetPostException<ExecutionOrder>(new Exception($"No se puede modificar el estado {type}, ya que ha finalizado la ejecucion.\n"));
            var modifier = await _graphApi.GetUserFromToken();
            var userActivity = new UserActivity(DateTime.Now,modifier);
            switch (type.ToLower()) {
                case "execution":
                    if (!Enum.IsDefined(typeof(ExecutionStatus), value))
                        error += "El nuevo valor de estado no esta definido en 'ExecutionStatus'.\n";
                    else if ((ExecutionStatus)value < execution.ExecutionStatus)
                        error += "El 'ExecutionStatus' no puede ser anterior al estado actual.\n";
                    else if ((ExecutionStatus)value > (execution.ExecutionStatus + 1))
                        error += "El 'ExecutionStatus' no puede ser posterior al siguiente estado.\n";
                    else if (value == 1) {
                        if (execution.UserApplicator == null)
                            error += "Para modificar el 'ExecutionStatus' a 'InProcess' se requiere un usuario aplicador.\n";
                        if (execution.ProductToApply == null || !execution.ProductToApply.Any())
                            error += "Para modificar el 'ExecutionStatus' a 'InProcess' se requiere un producto.\n";                        
                    }
                    break;
                case "finished":
                    if (!Enum.IsDefined(typeof(FinishStatus), value))
                        error += "El nuevo valor de estado no esta definido en 'FinishStatus'.\n";
                    else if ((int)execution.ExecutionStatus != 2)
                        error += "No se puede modificar el 'FinishStatus' mientras 'ExecutionStatus' aun no sea finalizado.\n";
                    break;
                case "closed":
                    if(!modifier.Roles.Any(role => role.Name.Equals("Administrador")))
                        error += "El 'ClosedStatus' solo puede ser modificado por un usuario con el rol 'Administrador'.\n";
                    else if (!Enum.IsDefined(typeof(ClosedStatus), value))
                        error += "El nuevo valor de estado no esta definido en 'ClosedStatus'.\n";
                    else if ((int)execution.ExecutionStatus != 3)
                        error += "No se puede modificar el 'ClosedStatus' mientras 'ExecutionStatus' aun no sea cerrado.\n";
                    break;
                default:
                    error += $"No existe el tipo de estado: {type}\n";
                    break;
            }
            if (!String.IsNullOrEmpty(error))
                return OperationHelper.GetPostException<ExecutionOrder>(new Exception(error));
            return await OperationHelper.EditElement(_commonDb, _repo.GetExecutionOrders(),
                idExecutionOrder,
                execution,
                s => {
                    switch (type.ToLower()) {
                        case "execution":
                            s.ExecutionStatus = (ExecutionStatus)value;
                            s.StatusInfo[value] = new Comments(userActivity, commentary);
                            break;
                        case "finished":
                            s.FinishStatus = (FinishStatus)value;
                            break;
                        case "closed":
                            s.ClosedStatus = (ClosedStatus)value;
                            break;
                    }
                    return s;
                },
                _repo.CreateUpdateExecutionOrder,
                 $"No existe orden de ejecucion con id: {idExecutionOrder}",
                s => false,
                ""
            );
        }

        public async Task<ExtPostContainer<ExecutionOrder>> AddCommentaryToExecutionOrder(string idExecutionOrder, string commentary) {
            if (String.IsNullOrWhiteSpace(idExecutionOrder)) return OperationHelper.GetPostException<ExecutionOrder>(new Exception("Es requerido 'idExecutionOrder'."));
            if (String.IsNullOrWhiteSpace(commentary)) return OperationHelper.GetPostException<ExecutionOrder>(new Exception("El comentario no puede estar vacio."));
            ExecutionOrder execution = await _repo.GetExecutionOrder(idExecutionOrder);
            if (execution == null)
                return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró la ejecucion con id {idExecutionOrder}", idExecutionOrder);
            var commentator = await _graphApi.GetUserFromToken();
            var userActivity = new UserActivity(DateTime.Now, commentator);
            return await OperationHelper.EditElement(_commonDb, _repo.GetExecutionOrders(),
                idExecutionOrder,
                execution,
                s => {
                    s.Comments.Add(new Comments(userActivity, commentary));
                    return s;
                },
                _repo.CreateUpdateExecutionOrder,
                 $"No existe orden de ejecucion con id: {idExecutionOrder}",
                s => false,
                ""
            );
        }

        public async Task<ExtGetContainer<SearchResult<ExecutionOrder>>> GetExecutionOrdersByPage(int page, int quantity, bool orderByDesc) {
            try {
                var executionsList = _repo.GetExecutionOrders();
                var paginatedExecutions = _commonDb.WithPagination(executionsList, page, quantity);
                var executionOrders = orderByDesc ? await _commonDb.TolistAsync(paginatedExecutions.OrderByDescending(s => s.Name)) : await _commonDb.TolistAsync(paginatedExecutions);
                var total = await _repo.Total(_idSeason);
                return OperationHelper.GetElement(new SearchResult<ExecutionOrder> {
                    Total = total,
                    Elements = executionOrders.ToArray()
                });
            }
            catch (Exception e) {
                return OperationHelper.GetException<SearchResult<ExecutionOrder>>(e);
            }
        }

        public async Task<ExtGetContainer<SearchResult<ExecutionOrder>>> GetExecutionOrdersByPage(string textToSearch, int page, int quantity, bool desc) {
            if (string.IsNullOrWhiteSpace(textToSearch))
                return await GetExecutionOrdersByPage(page, quantity, desc);
            EntitiesSearchContainer entitySearch = _searchServiceInstance.GetSearchFilteredByEntityName(entityName, textToSearch, page, quantity, desc);
            var resultDb = entitySearch.Entities.Select(async s => await GetExecutionOrder(s.Id));
            return OperationHelper.GetElement(new SearchResult<ExecutionOrder> {
                Total = entitySearch.Total,
                Elements = resultDb.Select(s => s.Result.Result).ToArray()
            });
        }

        public ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, int page, int quantity, bool desc) {
            EntitiesSearchContainer entitySearch = _searchServiceInstance.GetSearchFilteredByEntityName(entityName, textToSearch, page, quantity, desc);
            return OperationHelper.GetElement(entitySearch);
        }

    }
}

/*Ejecucion
* Anadir campos (producto,cantidad) relacionados a la orden.
* Es necesario crear una ruta para obtener lista de usuarios aplicadores
* Es necesario crear una ruta para obtener ejecuciones en proceso (Transversal a las ordenes)
* Es necesario crear una ruta para obtener ordenes que contengan ejecuciones en proceso
* Cuando la ejecucion cambie su executionStatus a 1:InProcess, se copiaran la fecha inicial y final de la orden a si misma.
* Cada vez que se setee el executionStatus (Inicialmente y sus sucesivos cambios), se debe almacenar la fecha (ExecutionStatusDate)
* El nuevo executionStatus debe ser siempre igual o superior al anterior (Como maximo en una unidad, ya que este estado es serial)
* EL finishStatus solo se puede setear cuando el executionStatus tiene el valor 2:EndProcess
* El closedStatus solo se puede setear cuando el executionStatus tiene el valor 3:Closed
* Al crear una ejecucion (En planificacion) es obligatoria la orden relacionada.
* Al iniciar la ejecucion (En proceso) el usuario aplicador asignado es obligatorio.
* El closedStatus solo puede ser seteado si el usuario posee el rol de "Administrador".
* Comentarios para cada estado, independiente de los comentarios transversales.
* Si la ejecucion ya finalizo(finishStatus != 0) solo se pueden recibir comentarios y cierre de ejecucion(set closedStatus to != 0)
* Si la orden relacionada ya posee una ejecucion exitosa no se puede crear una nueva ejecucion.*/