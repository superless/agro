using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.common;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external.Input;
using trifenix.agro.model.external.output;
using trifenix.agro.model.external;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.util;

namespace trifenix.agro.external.operations.entities.orders {
    public class ApplicationOrderOperations<T> : IApplicationOrderOperations where T : ApplicationOrder{

        private readonly IApplicationOrderRepository ApplicationOrder;
        private readonly IApplicationTargetRepository Target;
        private readonly IBarrackRepository Barracks;
        private readonly ICertifiedEntityRepository CertifiedEntity;
        private readonly ICommonDbOperations<T> CommonDb;
        private readonly IExecutionOrderRepository Execution;
        private readonly IGraphApi GraphApi;
        private readonly INotificationEventRepository Notifications;
        private readonly IPhenologicalPreOrderRepository PreOrder;
        private readonly IProductRepository Product;
        private readonly ISpecieRepository Specie;
        private readonly IVarietyRepository Variety;
        private readonly IAgroSearch SearchServiceInstance;
        private readonly string EntityName;
        private readonly string SeasonId;

        public ApplicationOrderOperations(IApplicationOrderRepository _applicationOrder, IApplicationTargetRepository _target, IBarrackRepository _barracks, ICertifiedEntityRepository _certifiedEntity, ICommonDbOperations<T> _commonDb, IExecutionOrderRepository _execution, IGraphApi _graphApi, INotificationEventRepository _notifications, IPhenologicalPreOrderRepository _preOrder, IProductRepository _product, ISpecieRepository _specie, IVarietyRepository _variety, string _seasonId, IAgroSearch _searchServiceInstance) {
            ApplicationOrder = _applicationOrder;
            Target = _target;
            Barracks = _barracks;
            CertifiedEntity = _certifiedEntity;
            CommonDb = _commonDb;
            Execution = _execution;
            GraphApi = _graphApi;
            Notifications = _notifications;
            PreOrder = _preOrder;
            Specie = _specie;
            Variety = _variety;
            Product = _product;
            EntityName = typeof(T).Name;
            SeasonId = _seasonId;
            SearchServiceInstance = _searchServiceInstance;
        }

        private OutPutApplicationOrder GetOutputOrder(ApplicationOrder appOrder) =>
            new OutPutApplicationOrder {
                Id = appOrder.Id,
                Correlative = appOrder.Correlative,
                Specie = appOrder.SpecieAbb,
                Wetting = appOrder.Wetting,
                Name = appOrder.Name,
                isPhenological = appOrder.IsPhenological,
                InitDate = appOrder.InitDate,
                EndDate = appOrder.EndDate,
                SeasonId = appOrder.SeasonId,
                ApplicationInOrders = appOrder.ApplicationInOrders.Select(async s => {
                    return new OutPutApplicationInOrder {
                        Doses = s.Doses,
                        Product = await Product.GetProduct(s.ProductId),
                        ProductId = s.ProductId,
                        QuantityByHectare = s.QuantityByHectare
                    };
                }).Select(s => s.Result).ToList(),
                PhenologicalPreOrders = appOrder.PhenologicalPreOrders,
                Barracks = appOrder.Barracks.Select(async s => {
                    var events = await s.EventsId.SelectElement(Notifications.GetNotificationEvent, "Identicadores de evento no encontrados");
                    return new OutputBarrackInstance {
                        Barrack = s.Barrack,
                        EventsId = s.EventsId,
                        Events = events.Select(a => new OutputOrderNotificationEvent {
                            Created = a.Created,
                            Description = a.Description,
                            Id = a.Id,
                            PhenologicalEvent = a.PhenologicalEvent,
                            PicturePath = a.PicturePath
                        }).ToList()
                    };
                }).Select(s => s.Result).ToList()
            };

        public async Task<ExtGetContainer<OutPutApplicationOrder>> GetApplicationOrder(string id) {
            try {
                var appOrder = await ApplicationOrder.GetApplicationOrder(id);
                var newAppOrder = GetOutputOrder(appOrder);
                return OperationHelper.GetElement(newAppOrder);
            }
            catch (Exception e) {
                return OperationHelper.GetException<OutPutApplicationOrder>(e);
            }
        }

        public async Task<ExtPostContainer<OutPutApplicationOrder>> SaveEditApplicationOrder(string id, ApplicationOrderInput input) {
            var modifier = await GraphApi.GetUserFromToken();
            var userActivity = new UserActivity(DateTime.Now, modifier);
            T order = (T)await ApplicationOrder.GetApplicationOrder(id);
            var newAppOrder = await GetApplicationOrder(id, input);
            //bool changedSpecie = !order.SpecieAbb.Equals(newAppOrder.SpecieAbb);
            var executions = GetExecutionsInOrder(id);
            //TODO: Validaciones al momento de modificar orden
            //LISTO ->  Al modificar la fecha en la orden, no deben existir ejecuciones en proceso,  todas deben estar cerradas.   Posible duplicidad 
            //LISTO -> No se puede modificar una orden que ya posee una ejecucion en proceso o una ejecucion exitosa (cerrada).    
            if (executions.Result.Any(execution => execution.ExecutionStatus == ExecutionStatus.InProcess || execution.FinishStatus == FinishStatus.Successful)) return OperationHelper.GetPostException<OutPutApplicationOrder>(new Exception("No se puede modificar una Orden de Aplicacion que posee al menos una Ejecucion en proceso o que ha finalizado con exito."));
            if ((DateTime.Compare(order.InitDate,newAppOrder.InitDate) != 0 || DateTime.Compare(order.EndDate,newAppOrder.EndDate) != 0) && executions.Result.Any(execution => execution.ClosedStatus != 0)) return OperationHelper.GetPostException<OutPutApplicationOrder>(new Exception("Para modificar la fecha de inicio o fecha de fin en la Orden de Aplicacion, todas las Ejecuciones deben estar cerradas."));
            var editOperation = await OperationHelper.EditElement(CommonDb, (IQueryable<T>)ApplicationOrder.GetApplicationOrders(),
                id,
                order,
                s => {
                    newAppOrder.Creator = s.Creator;
                    newAppOrder.ModifyBy = s.ModifyBy;
                    newAppOrder.ModifyBy.Add(userActivity);
                    //if (changedSpecie)
                    //    newAppOrder.Correlative = CommonDb.GetCorrelativePosition(newAppOrder.SpecieAbb);
                    return newAppOrder;
                },
                ApplicationOrder.CreateUpdate,
                $"No existe orden con id {id}",
                s => s.Name.Equals(input.Name) && input.Name != order.Name,
                $"Ya existe orden de aplicacion con nombre : {input.Name}"
            );
            if (editOperation.GetType() == typeof(ExtPostErrorContainer<string>))
                return OperationHelper.GetPostException<OutPutApplicationOrder>(new Exception(editOperation.Message));
            //if (changedSpecie)
            //    CommonDb.IncreaseCorrelativePosition(newAppOrder.SpecieAbb);
            SearchServiceInstance.AddEntities(new List<EntitySearch> {
                new EntitySearch{
                    Id = id,
                    Name = input.Name,
                    Specie = newAppOrder.SpecieAbb,
                    Type = input.isPhenological
                }
            });
            return new ExtPostContainer<OutPutApplicationOrder> {
                IdRelated = editOperation.IdRelated,
                Message = editOperation.Message,
                MessageResult = editOperation.MessageResult,
                Result = GetOutputOrder(newAppOrder)
            };
        }

        private async Task<T> GetApplicationOrder(string id, ApplicationOrderInput input) {
            var targetIds = input.Applications.Any(s => s.Doses != null) ? input.Applications.Where(s => s.Doses != null).SelectMany(s => s.Doses.idsApplicationTarget).Distinct() : new List<string>();
            var varietyAbb = new List<string> { Barracks.GetBarrack(input.BarracksInput.FirstOrDefault()?.IdBarrack).Result.Variety?.Abbreviation };
            var specieAbb = Barracks.GetBarrack(input.BarracksInput.FirstOrDefault()?.IdBarrack).Result.Variety.Specie.Abbreviation;
            //int correlative = CommonDb.GetCorrelativePosition(specieAbb);
            var certifiedEntitiesIds = input.Applications.Any(s => s.Doses != null) ? input.Applications.Where(s => s.Doses != null).SelectMany(s => s.Doses.WaitingHarvest.Select(a => a.IdCertifiedEntity)).Distinct() : new List<string>();
            var barracksInstances = await GetBarracksIntance(input.BarracksInput);
            var applications = GetApplicationInOrder(input.Applications);
            var phenologicalPreOrders = input.PreOrdersId == null || !input.PreOrdersId.Any() ? new List<PhenologicalPreOrder>() :
            await input.PreOrdersId.SelectElement(PreOrder.GetPhenologicalPreOrder, "Existen identificadores de preordenes que no fueron encontrados");
            var creator = await GraphApi.GetUserFromToken();
            var userActivity = new UserActivity(DateTime.Now, creator);
            return (T)Activator.CreateInstance(typeof(T), new object[] {
                id,
                0,
                certifiedEntitiesIds?.ToList(),
                specieAbb,
                barracksInstances,
                targetIds?.ToList(),
                varietyAbb?.ToList(),
                SeasonId,
                input.Name,
                input.isPhenological,
                input.InitDate,
                input.EndDate,
                input.Wetting,
                applications,
                userActivity,
                phenologicalPreOrders
            });
                
        }

        public async Task<ExtPostContainer<string>> SaveNewApplicationOrder(ApplicationOrderInput input) {
            var createOperation = await OperationHelper.CreateElement(CommonDb, (IQueryable<T>)ApplicationOrder.GetApplicationOrders(),
                async s => {
                    ApplicationOrder order = await GetApplicationOrder(s, input);
                    order.InnerCorrelative = 1;
                    return await ApplicationOrder.CreateUpdate(order);
                },
                s => s.Name.Equals(input.Name),
                $"Ya existe orden de aplicacion con nombre: {input.Name}");
            if (createOperation.GetType() == typeof(ExtPostErrorContainer<string>))
                return OperationHelper.GetPostException<string>(new Exception(createOperation.Message));
            string specieAbb = Barracks.GetBarrack(input.BarracksInput.FirstOrDefault()?.IdBarrack).Result.Variety.Specie.Abbreviation;
            SearchServiceInstance.AddEntities(new List<EntitySearch> {
                new EntitySearch {
                    Id = createOperation.IdRelated,
                    SeasonId = SeasonId,
                    Created = DateTime.Now,
                    EntityName = EntityName,
                    Name = input.Name,
                    Specie = specieAbb,
                    Type = input.isPhenological
                }
            });
            //try {
            //    CommonDb.IncreaseCorrelativePosition(specieAbb);
            //}
            //catch (Exception e) {
            //    return OperationHelper.PostNotFoundElementException<string>(e.Message);
            //}
            return createOperation;
        }

        private List<ApplicationsInOrder> GetApplicationInOrder(ApplicationInOrderInput[] appInOrder) {
            return appInOrder.Select(async s => {
                if (s.Doses == null) {
                    return new ApplicationsInOrder {
                        ProductId = s.ProductId,
                        QuantityByHectare = s.QuantityByHectare
                    };
                }
                var dose = await GetDose(s.Doses);
                return new ApplicationsInOrder {
                    ProductId = s.ProductId,
                    QuantityByHectare = s.QuantityByHectare,
                    Doses = dose
                };
            }).Select(s => s.Result).ToList();
        }

        public ExtGetContainer<List<ExecutionOrder>> GetExecutionsInOrder(string idOrder) {
            var executions = Execution.GetExecutionOrders(idOrder);
            return OperationHelper.GetElements(executions.ToList());
        }

        private async Task<Doses> GetDose(DosesInput input) {
            var doses = new List<DosesInput> { input };
            var idCerts = input.WaitingHarvest.Select(s => s.IdCertifiedEntity).Distinct();
            var dosesResult = await ModelCommonOperations.GetDoses(Variety, Target, Specie, CertifiedEntity, doses.ToArray(), input.IdVarieties, input.idsApplicationTarget, input.IdSpecies, idCerts, SeasonId);
            return dosesResult.First();
        }

        private async Task<List<BarrackOrderInstance>> GetBarracksIntance(BarrackEventInput[] barracksInput) {
            var barracks = await barracksInput.Select(s => s.IdBarrack).SelectElement(Barracks.GetBarrack, "Uno de los identificadores de cuartel no fue encontrado");
            return barracksInput.Select(s => {
                return new BarrackOrderInstance {
                    Barrack = barracks.First(a => a.Id.Equals(s.IdBarrack)),
                    EventsId = s.EventsId?.ToList()
                };
            }).ToList();
        }

        public async Task<ExtGetContainer<List<OutPutApplicationOrder>>> GetApplicationOrders() {
            try {
                var applicationOrderQuery = ApplicationOrder.GetApplicationOrders();
                var applicationOrders = await CommonDb.TolistAsync((IQueryable<T>)applicationOrderQuery);
                var outputOrders = applicationOrders.Select(GetOutputOrder).ToList();
                return OperationHelper.GetElements(outputOrders);
            }
            catch (Exception e) {
                return OperationHelper.GetException<List<OutPutApplicationOrder>>(e);
            }
        }
        
        public ExtGetContainer<SearchResult<OutPutApplicationOrder>> GetPaginatedOrders(string textToSearch, string abbSpecie, bool? type, int? page, int? quantity, bool? desc) {
            var filters = new Filters { EntityName = EntityName, SeasonId = SeasonId };
            if (!string.IsNullOrWhiteSpace(abbSpecie))
                filters.Specie = abbSpecie;
            if (type.HasValue)
                filters.Type = type;
            var parameters = new Parameters { Filters = filters, TextToSearch = textToSearch, Page = page, Quantity = quantity, Desc = desc };
            EntitiesSearchContainer entitySearch = SearchServiceInstance.GetPaginatedEntities(parameters);
            var resultDb = entitySearch.Entities.Select(async order => await GetApplicationOrder(order.Id));
            return OperationHelper.GetElement(new SearchResult<OutPutApplicationOrder> {
                Total = entitySearch.Total,
                Elements = resultDb.Select(s=>s.Result.Result).ToArray()
            });
        }

        public ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, string abbSpecie, bool? type, int? page, int? quantity, bool? desc) {
            var filters = new Filters { EntityName = EntityName, SeasonId = SeasonId };
            if (!string.IsNullOrWhiteSpace(abbSpecie))
                filters.Specie = abbSpecie;
            if (type.HasValue)
                filters.Type = type;
            var parameters = new Parameters { Filters = filters, TextToSearch = textToSearch, Page = page, Quantity = quantity, Desc = desc };
            EntitiesSearchContainer entitySearchFilteresBySeason = SearchServiceInstance.GetPaginatedEntities(parameters);
            return OperationHelper.GetElement(entitySearchFilteresBySeason);
        }

    }
}