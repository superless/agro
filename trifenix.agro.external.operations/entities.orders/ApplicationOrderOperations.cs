using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.common;
using trifenix.agro.external.operations.entities.orders.args;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.model.external.output;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.util;

namespace trifenix.agro.external.operations.entities.orders {
    public class ApplicationOrderOperations<T> : IApplicationOrderOperations where T : ApplicationOrder{
        private readonly ApplicationOrderArgs _args;
        private readonly IAgroSearch _searchServiceInstance;
        private readonly string entityName = typeof(T).Name;
        public ApplicationOrderOperations(ApplicationOrderArgs args, IAgroSearch searchServiceInstance) {
            _args = args;
            _searchServiceInstance = searchServiceInstance;
        }

        private OutPutApplicationOrder GetOutputOrder(ApplicationOrder appOrder) {
            return new OutPutApplicationOrder {
                Id = appOrder.Id,
                Wetting = appOrder.Wetting,
                Name = appOrder.Name,
                isPhenological = appOrder.IsPhenological,
                InitDate = appOrder.InitDate,
                EndDate = appOrder.EndDate,
                SeasonId = appOrder.SeasonId,
                ApplicationInOrders = appOrder.ApplicationInOrders.Select(async s => {
                    return new OutPutApplicationInOrder {
                        Doses = s.Doses,
                        Product = await _args.Product.GetProduct(s.ProductId),
                        ProductId = s.ProductId,
                        QuantityByHectare = s.QuantityByHectare
                    };
                }).Select(s => s.Result).ToList(),
                PhenologicalPreOrders = appOrder.PhenologicalPreOrders,
                Barracks = appOrder.Barracks.Select(async s => {
                    var events = await s.EventsId.SelectElement(_args.Notifications.GetNotificationEvent, "Identicadores de evento no encontrados");
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
        }

        public async Task<ExtGetContainer<OutPutApplicationOrder>> GetApplicationOrder(string id) {
            try {
                var appOrder = await _args.ApplicationOrder.GetApplicationOrder(id);
                var newAppOrder = GetOutputOrder(appOrder);
                return OperationHelper.GetElement(newAppOrder);
            }
            catch (Exception e) {
                return OperationHelper.GetException<OutPutApplicationOrder>(e);
            }
        }

        public async Task<ExtPostContainer<OutPutApplicationOrder>> SaveEditApplicationOrder(string id, ApplicationOrderInput input) {
            var modifier = await _args.GraphApi.GetUserFromToken();
            
            T order = (T)await _args.ApplicationOrder.GetApplicationOrder(id);
            var appNewOrder = await GetApplicationOrder(id, input);
            var editOperation = await OperationHelper.EditElement(_args.CommonDb.ApplicationOrder, _args.ApplicationOrder.GetApplicationOrders(),
                id,
                order,
                s => {
                    _searchServiceInstance.AddElements(new List<EntitySearch> {
                        new EntitySearch{
                            Id = id,
                            Name = input.Name,
                            Specie = _args.Barracks.GetBarrack(input.BarracksInput.FirstOrDefault()?.IdBarrack).Result.Variety.Specie.Abbreviation,
                            Type = input.isPhenological
                        }
                    });
                    
                    
                    return appNewOrder;
                },
                _args.ApplicationOrder.CreateUpdate,
                $"No existe orden con id {id}",
                s => s.Name.Equals(input.Name) && input.Name != order.Name,
                $"Ya existe orden de aplicacion con nombre : {input.Name}"
            );
            if (editOperation.GetType() == typeof(ExtPostErrorContainer<string>))
                return OperationHelper.GetPostException<OutPutApplicationOrder>(new Exception(editOperation.Message));
            return new ExtPostContainer<OutPutApplicationOrder> {
                IdRelated = editOperation.IdRelated,
                Message = editOperation.Message,
                MessageResult = editOperation.MessageResult,
                Result = GetOutputOrder(appNewOrder)
            };
        }

        private async Task<T> GetApplicationOrder(string id, ApplicationOrderInput input) {
            var varietyIds = input.Applications.Any(s => s.Doses != null) ? input.Applications.Where(s => s.Doses != null).SelectMany(s => s.Doses.IdVarieties).Distinct() : new List<string>();
            var targetIds = input.Applications.Any(s => s.Doses != null) ? input.Applications.Where(s => s.Doses != null).SelectMany(s => s.Doses.idsApplicationTarget).Distinct() : new List<string>();
            var speciesIds = input.Applications.Any(s => s.Doses != null) ? input.Applications.Where(s => s.Doses != null).SelectMany(s => s.Doses.IdSpecies).Distinct() : new List<string>();
            var certifiedEntitiesIds = input.Applications.Any(s => s.Doses != null) ? input.Applications.Where(s => s.Doses != null).SelectMany(s => s.Doses.WaitingHarvest.Select(a => a.IdCertifiedEntity)).Distinct() : new List<string>();
            var barracksInstances = await GetBarracksIntance(input.BarracksInput);
            var applications = GetApplicationInOrder(input.Applications);
            var phenologicalPreOrders = input.PreOrdersId == null || !input.PreOrdersId.Any() ? new List<PhenologicalPreOrder>() :
            await input.PreOrdersId.SelectElement(_args.PreOrder.GetPhenologicalPreOrder, "Existen identificadores de preordenes que no fueron encontrados");
            var creator = await _args.GraphApi.GetUserFromToken();
            
            return (T)Activator.CreateInstance(typeof(T), new object[] {
                id,
                certifiedEntitiesIds?.ToList(),
                speciesIds?.ToList(),
                barracksInstances,
                targetIds?.ToList(),
                varietyIds?.ToList(),
                _args.SeasonId,
                input.Name,
                input.isPhenological,
                input.InitDate,
                input.EndDate,
                input.Wetting,
                applications,
                phenologicalPreOrders
            });
                
        }

        //TODO: Validaciones al momento de modificar orden
        //Al modificar la fecha en la orden, no deben existir ejecuciones en proceso,  todas deben estar cerradas.   Posible duplicidad 
        //No se puede modificar una orden que ya posee una ejecucion en proceso o una ejecucion exitosa (cerrada).           ^

        public async Task<ExtPostContainer<string>> SaveNewApplicationOrder(ApplicationOrderInput input) {
            var createOperation = await OperationHelper.CreateElement(_args.CommonDb.ApplicationOrder, _args.ApplicationOrder.GetApplicationOrders(),
                async s => await _args.ApplicationOrder.CreateUpdate(await GetApplicationOrder(s, input)),
                    s => s.Name.Equals(input.Name),
                    $"Ya existe orden de aplicacion con nombre: {input.Name}");
            if (createOperation.GetType() == typeof(ExtPostErrorContainer<string>))
                return OperationHelper.GetPostException<string>(new Exception(createOperation.Message));
            _searchServiceInstance.AddElements(new List<EntitySearch> {
                new EntitySearch {
                    Id = createOperation.IdRelated,
                    SeasonId = _args.SeasonId,
                    Created = DateTime.Now,
                    EntityIndex = entityName,
                    Name = input.Name,
                    Specie = _args.Barracks.GetBarrack(input.BarracksInput.FirstOrDefault()?.IdBarrack).Result.Variety.Specie.Abbreviation,
                    Type = input.isPhenological
                }
            });
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

        private async Task<Doses> GetDose(DosesInput input) {
            var doses = new List<DosesInput> { input };
            var idCerts = input.WaitingHarvest.Select(s => s.IdCertifiedEntity).Distinct();
            var dosesResult = await ModelCommonOperations.GetDoses(_args.DosesArgs.Variety, _args.DosesArgs.Target, _args.DosesArgs.Specie, _args.DosesArgs.CertifiedEntity, doses.ToArray(), input.IdVarieties, input.idsApplicationTarget, input.IdSpecies, idCerts, _args.SeasonId);
            return dosesResult.First();
        }

        private async Task<List<BarrackOrderInstance>> GetBarracksIntance(BarrackEventInput[] barracksInput) {
            var barracks = await barracksInput.Select(s => s.IdBarrack).SelectElement(_args.Barracks.GetBarrack, "Uno de los identificadores de cuartel no fue encontrado");
            return barracksInput.Select(s => {
                return new BarrackOrderInstance {
                    Barrack = barracks.First(a => a.Id.Equals(s.IdBarrack)),
                    EventsId = s.EventsId?.ToList()
                };
            }).ToList();
        }

        public async Task<ExtGetContainer<List<OutPutApplicationOrder>>> GetApplicationOrders() {
            try {
                var applicationOrderQuery = _args.ApplicationOrder.GetApplicationOrders();
                var applicationOrders = await _args.CommonDb.ApplicationOrder.TolistAsync(applicationOrderQuery);
                var outputOrders = applicationOrders.Select(GetOutputOrder).ToList();
                return OperationHelper.GetElements(outputOrders);
            }
            catch (Exception e) {
                return OperationHelper.GetException<List<OutPutApplicationOrder>>(e);
            }
        }
        
        public ExtGetContainer<SearchResult<OutPutApplicationOrder>> GetPaginatedOrders(string textToSearch, string abbSpecie, bool? type, int? page, int? quantity, bool? desc) {
            var filters = new Filters { EntityName = entityName, SeasonId = _args.SeasonId };
            if (!string.IsNullOrWhiteSpace(abbSpecie))
                filters.Specie = abbSpecie;
            if (type.HasValue)
                filters.Type = type;
            var parameters = new Parameters { Filters = filters, TextToSearch = textToSearch, Page = page, Quantity = quantity, Desc = desc };
            EntitiesSearchContainer entitySearch = _searchServiceInstance.GetPaginatedEntities(parameters);
            var resultDb = entitySearch.Entities.Select(async order => await GetApplicationOrder(order.Id));
            return OperationHelper.GetElement(new SearchResult<OutPutApplicationOrder> {
                Total = entitySearch.Total,
                Elements = resultDb.Select(s=>s.Result.Result).ToArray()
            });
        }

        public ExtGetContainer<EntitiesSearchContainer> GetIndexElements(string textToSearch, string abbSpecie, bool? type, int? page, int? quantity, bool? desc) {
            var filters = new Filters { EntityName = entityName, SeasonId = _args.SeasonId };
            if (!string.IsNullOrWhiteSpace(abbSpecie))
                filters.Specie = abbSpecie;
            if (type.HasValue)
                filters.Type = type;
            var parameters = new Parameters { Filters = filters, TextToSearch = textToSearch, Page = page, Quantity = quantity, Desc = desc };
            EntitiesSearchContainer entitySearchFilteresBySeason = _searchServiceInstance.GetPaginatedEntities(parameters);
            return OperationHelper.GetElement(entitySearchFilteresBySeason);
        }

    }
}