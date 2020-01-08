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

        public ExecutionOrderOperations(IExecutionOrderRepository repo, IApplicationOrderRepository repoOrders, IUserRepository repoUsers, INebulizerRepository repoNebulizers, IProductRepository repoProducts, ITractorRepository repoTractors, ICommonDbOperations<ExecutionOrder> commonDb, IGraphApi graphApi) {
            _repo = repo;
            _repoOrders = repoOrders;
            _repoUsers = repoUsers;
            _repoNebulizers = repoNebulizers;
            _repoProducts = repoProducts;
            _repoTractors = repoTractors;
            _commonDb = commonDb;
            _graphApi = graphApi;
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

        public async Task<ExtPostContainer<string>> SaveNewExecutionOrder(string idOrder, string idUserApplicator, string idNebulizer, string idProduct, double quantityByHectare, string idTractor, string commentary) {
            if (string.IsNullOrWhiteSpace(idOrder)) return OperationHelper.GetPostException<string>(new Exception("Es requerido 'idOrder' para crear una ejecucion."));
            ApplicationOrder order = await _repoOrders.GetApplicationOrder(idOrder);
            if (order == null)
                return OperationHelper.PostNotFoundElementException<string>($"No se encontró la orden de aplicacion con id {idOrder}", idOrder);
            UserApplicator userApplicator = null;
            Nebulizer nebulizer = null;
            Product product = null;
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
            if (!String.IsNullOrWhiteSpace(idProduct)) {
                product = await _repoProducts.GetProduct(idProduct);
                if (product == null)
                    return OperationHelper.PostNotFoundElementException<string>($"No se encontró el producto con id {idProduct}", idProduct);
            }
            if (!String.IsNullOrWhiteSpace(idTractor)) {
                tractor = await _repoTractors.GetTractor(idTractor);
                if (tractor == null)
                    return OperationHelper.PostNotFoundElementException<string>($"No se encontró el tractor con id {idTractor}", idTractor);
            }
            UserApplicator creator = await _graphApi.GetUserFromToken();
            UserActivity userActivity = new UserActivity(DateTime.Now, creator);
            var createOperation = await OperationHelper.CreateElement(_commonDb, _repo.GetExecutionOrders(),
               async s => await _repo.CreateUpdateExecutionOrder(new ExecutionOrder {
                   Id = s,
                   Order = order,
                   UserApplicator = userApplicator,
                   Nebulizer = nebulizer,
                   ProductToApply = new ProductToApply() { Product = product, QuantityByHectare = quantityByHectare },
                   Tractor = tractor,
                   Creator = userActivity
               }),
               s => false,
               ""
            );
            var setStatusOperation = await SetStatus(createOperation.IdRelated, "execution", 0, commentary);
            return new ExtPostContainer<string>
            {
                IdRelated = setStatusOperation.IdRelated,
                Result = setStatusOperation.IdRelated,
                MessageResult = ExtMessageResult.Ok
            };
        }

        public async Task<ExtPostContainer<ExecutionOrder>> SaveEditExecutionOrder(string id, string idOrder, string idUserApplicator, string idNebulizer, string idProduct, double quantityByHectare, string idTractor) {
            ExecutionOrder execution = await _repo.GetExecutionOrder(id);
            if(execution.ExecutionStatus > 0)
                return OperationHelper.GetPostException<ExecutionOrder>(new Exception("Solo se puede modificar la ejecucion cuando esta en planificacion."));
            if (string.IsNullOrWhiteSpace(idOrder)) return OperationHelper.GetPostException<ExecutionOrder>(new Exception("Es requerido 'idOrder'."));
            ApplicationOrder order = await _repoOrders.GetApplicationOrder(idOrder);
            if (order == null)
                return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró la orden de aplicacion con id {idOrder}", idOrder);
            UserApplicator userApplicator = null;
            Nebulizer nebulizer = null;
            Product product = null;
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
            if (!String.IsNullOrWhiteSpace(idProduct)) {
                product = await _repoProducts.GetProduct(idProduct);
                if (product == null)
                    return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró el producto con id {idProduct}", idProduct);
            }
            if (!String.IsNullOrWhiteSpace(idTractor)) {
                tractor = await _repoTractors.GetTractor(idTractor);
                if (tractor == null)
                    return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró el tractor con id {idTractor}", idTractor);
            }
            UserApplicator modifier = await _graphApi.GetUserFromToken();
            UserActivity userActivity = new UserActivity(DateTime.Now, modifier);
            return await OperationHelper.EditElement(_commonDb, _repo.GetExecutionOrders(),
                id,
                execution,
                s => {
                    s.Order = order;
                    s.UserApplicator = userApplicator;
                    s.Nebulizer = nebulizer;
                    s.ProductToApply.Product = product;
                    s.ProductToApply.QuantityByHectare = quantityByHectare;
                    s.Tractor = tractor;
                    s.ModifyBy.Add(userActivity);
                    return s;
                },
                _repo.CreateUpdateExecutionOrder,
                 $"No existe orden de ejecucion con id: {id}",
                s => false,
                ""
            );
        }

        public async Task<ExtPostContainer<ExecutionOrder>> SetStatus(string idExecutionOrder, string type, int value, string commentary) {
            string error = "";
            if (string.IsNullOrWhiteSpace(idExecutionOrder)) error += "Es requerido 'idExecutionOrder' para crear una ejecucion.\n";
            if (string.IsNullOrWhiteSpace(type)) error += "Es requerido 'type' para crear una ejecucion.\n";
            if (!String.IsNullOrEmpty(error)) return OperationHelper.GetPostException<ExecutionOrder>(new Exception(error));
            ExecutionOrder execution = await _repo.GetExecutionOrder(idExecutionOrder);
            if (execution == null)
                return OperationHelper.PostNotFoundElementException<ExecutionOrder>($"No se encontró la ejecucion con id {idExecutionOrder}", idExecutionOrder);
            if (execution.FinishStatus != 0 && !type.ToLower().Equals("closed"))
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
                        if (execution.ProductToApply.Product == null)
                            error += "Para modificar el 'ExecutionStatus' a 'InProcess' se requiere un producto.\n";
                        if (execution.ProductToApply.QuantityByHectare <= 0)
                            error += "Para modificar el 'ExecutionStatus' a 'InProcess' se requiere una cantidad positiva para aplicar producto.\n";                                        
                    }
                    break;
                case "finished":
                    if (!Enum.IsDefined(typeof(FinishStatus), value))
                        error += "El nuevo valor de estado no esta definido en 'FinishStatus'.\n";
                    else if ((int)execution.ExecutionStatus != 2)
                        error += "No se puede modificar el 'FinishStatus' mientras 'ExecutionStatus' aun no sea finalizado.\n";
                    break;
                case "closed":
                    if(modifier.Roles.Any(role => role.Name.Equals("Administrador")))
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
        * Comentarios para cada estado, independiente de los comentarios transversales.*/

         //TODO: Validaciones faltantes
         //Si la orden relacionada ya posee una ejecucion exitosa no se puede crear una nueva ejecucion.       
         //Si la ejecucion ya finalizo (finishStatus != 0) solo se pueden recibir comentarios y cierre de ejecucion (set closedStatus to != 0)