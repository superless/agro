using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.enums;
using trifenix.agro.external.interfaces.custom;
using trifenix.agro.external.operations.custom.args;
using trifenix.agro.external.operations.helper;
using trifenix.agro.model.external;
using trifenix.agro.model.external.output;
using trifenix.agro.util;

namespace trifenix.agro.external.operations.custom {

    public class MobileCustomElements : IMobileEventCustomElements {
        private readonly ArgsMobileCustom _args;

        public MobileCustomElements(ArgsMobileCustom args) {
            _args = args;
        }

        public async Task<ExtGetContainer<EventInitData>> GetEventData() {
            var barracksQuery = _args.Barrack.GetBarracks().Where(s => s.SeasonId == _args.IdSeason);
            var barracks = await _args.CommonDb.Barrack.TolistAsync(barracksQuery);
            var phenologicalQuery = _args.Phenological.GetPhenologicalEvents();
            var phenologicals = await _args.CommonDb.Phenological.TolistAsync(phenologicalQuery);
            var init = await GetEventInitData(barracks, phenologicals);
            return OperationHelper.GetElement(init);
        }

        private async Task<EventInitData> GetEventInitData(List<Barrack> barracks, List<Event> phenologicalEvents) {
            var tsResult = await GetMobileEventTimestamp();
            var sectors = GetSectors(barracks);
            return new EventInitData {
                TimeStamp = tsResult.Result,
                Sectors = sectors,
                Events = new Dictionary<int, OutputMobileEvent[]> {
                    { (int)KindEvent.Phenological ,phenologicalEvents.Select(s => new OutputMobileEvent {
                        Id = s.Id,
                        Name = s.Name
                    }).ToArray()}
                }
            };
        }

        public async Task<ExtGetContainer<long>> GetMobileEventTimestamp() {
            var tsBarracks = await _args.TimeStampDbQuery.GetTimestamps<Barrack>();
            var tsPhenological = await _args.TimeStampDbQuery.GetTimestamps<Event>();
            return OperationHelper.GetElement(tsBarracks.Union(tsPhenological).Max());
        }

        private OutputMobileSector[] GetSectors(List<Barrack> barracks) {
            return barracks.GroupBy(s => s.PlotLand.Sector.Id).Select(s => {
                var species = GetVarieties(s.ToList()).ToArray();
                return new OutputMobileSector {
                    Id = s.Key,
                    Name = s.First().PlotLand.Sector.Name,
                    Species = species
                };
            }).ToArray();
        }

        private async Task<OutputMobileSpecie> GetSpecie(string idVariety) {
            var variety = await _args.Variety.GetVariety(idVariety);
            return new OutputMobileSpecie {
                Id = variety.Specie.Id,
                Name = variety.Specie.Name
            };
        }

        private IEnumerable<OutputMobileSpecie> GetVarieties(List<Barrack> barracks) {
            var specieList = barracks.GroupBy(s => s.Variety.Id).Select(async s => {
                var specie = await GetSpecie(s.Key);
                return new OutputMobileSpecie {
                    Id = specie.Id,
                    Name = specie.Name,
                    Barracks = GetBarracks(s.ToList())
                };
            });
            return specieList.GroupBy(s => s.Result.Id).Select(v => new OutputMobileSpecie {
                Id = v.Key,
                Name = v.First().Result.Name,
                Barracks = v.SelectMany(a => a.Result.Barracks).GroupBy(b => b.Id).Select(f => f.First()).ToArray()
            });
        }

        private OutputMobileBarrack[] GetBarracks(List<Barrack> barracks) {
            return barracks.GroupBy(s => s.Id).Select(s => new OutputMobileBarrack {
                Id = s.Key,
                Name = s.First().Name
            }).ToArray();
        }

        public async Task<ExtGetContainer<NotificationCustomPhenologicalResult>> GetNotificationPreOrdersResult(string idSpecie, int page, int elementsByPage, bool orderDateDesc) {
            var notifications = await GetNotificationPreOrders(idSpecie, page, elementsByPage, orderDateDesc);
            var total = await _args.NotificationEvent.Total(_args.IdSeason, idSpecie);
            return OperationHelper.GetElement(new NotificationCustomPhenologicalResult {
                Notifications = notifications,
                Total = total
            });
        }

        #region private methods
        private async Task<OutPutNotificationPreOrder[]> GetNotificationPreOrders(string idSpecie, int page, int elementsByPage, bool orderDateDesc) {
            var notifications = await GetNotificationEvents(idSpecie, page, elementsByPage, orderDateDesc);
            return GetNotificationPreOrders(notifications.ToList()).ToArray();
        }

        private IEnumerable<OutPutNotificationPreOrder> GetNotificationPreOrders(List<NotificationEvent> notifications) {
            return notifications.Select(async s => {
                var preOrders = await GetOutPutPhenologicalPreOrders(s.PhenologicalEvent.Id, s.Barrack.Id);
                return new OutPutNotificationPreOrder {
                    Id = s.Id,
                    Barrack = s.Barrack,
                    CosmosEntityName = s.CosmosEntityName,
                    Created = s.Created,
                    Description = s.Description,
                    PhenologicalEvent = s.PhenologicalEvent,
                    PicturePath = s.PicturePath,
                    PreOrders = preOrders.ToArray()
                };
            }).Select(s => s.Result);
        }

        private async Task<IEnumerable<NotificationEvent>> GetNotificationEvents(string idSpecie, int page, int elementsByPage, bool orderDateDesc) {
            //var notificationsQuery = _args.NotificationEvent.GetNotificationEvents().Where(s => s.Barrack.SeasonId.Equals(_args.IdSeason));
            var notificationsQuery = _args.NotificationEvent.GetNotificationEvents().Where(s => s.Barrack.SeasonId.Equals(_args.IdSeason) && s.Barrack.Variety.Specie.Id.Equals(idSpecie));
            var notificationQueryWithPagination = _args.CommonDb.NotificationEvent.WithPagination(notificationsQuery, page, elementsByPage);
            if (orderDateDesc)
                return await _args.CommonDb.NotificationEvent.TolistAsync(notificationQueryWithPagination.OrderByDescending(s => s.Created));
            return await _args.CommonDb.NotificationEvent.TolistAsync(notificationQueryWithPagination.OrderBy(s => s.Created));
        }

        private async Task<IEnumerable<NotificationEvent>> GetNotificationEvents(string idBarrack, string idPhenologicalEvent) {
            var notificationsQuery = _args.NotificationEvent.GetNotificationEvents().Where(s => s.Barrack.SeasonId.Equals(_args.IdSeason) && s.PhenologicalEvent.Id.Equals(idPhenologicalEvent) && s.Barrack.Id.Equals(idBarrack));
            return await _args.CommonDb.NotificationEvent.TolistAsync(notificationsQuery);
        }

        private async Task<OutputNotificationEvent[]> GetOutputNotificationEvents(string idBarrack, string idPhenologicalEvent) {
            var notifications = await GetNotificationEvents(idBarrack, idPhenologicalEvent);
            return notifications.Select(s => new OutputNotificationEvent {
                Id = s.Id,
                Description = s.Description,
                PicturePath = s.PicturePath,
                Created = s.Created
            }).ToArray();
        }

        private async Task<OutPutPhenologicalPreOrder[]> GetOutPutPhenologicalPreOrders(string idPhenologicalEvent, string idBarrack) {
            var phenologicalPreOrders = await GetPhenologicalPreOrders(idPhenologicalEvent, idBarrack);
            return phenologicalPreOrders.Select(async s => {
                var barracks = await GetOutputBarracks(s.BarracksId.ToArray(), idPhenologicalEvent);
                return new OutPutPhenologicalPreOrder {
                    Id = s.Id,
                    Name = s.Name,
                    SeasonId = s.SeasonId,
                    Barracks = barracks.ToArray()

                };
            }).Select(s => s.Result).ToArray();
        }

        private async Task<IEnumerable<PhenologicalPreOrder>> GetPhenologicalPreOrders(string idPhenologicalEvent, string idBarrack) {
            var ordersId = await GetOrderFoldersIds(idPhenologicalEvent);
            var phenologicalPreOrdersQuery = _args.PhenologicalPreOrder.GetPhenologicalPreOrders().
                Where(s => s.SeasonId.Equals(_args.IdSeason) && s.BarracksId.Any(a => a.Equals(idBarrack)));
            var result = await _args.CommonDb.PhenologicalPreOrder.TolistAsync(phenologicalPreOrdersQuery);
            return result.Where(s => ordersId.Any(v => v.Equals(s.OrderFolderId)));
        }

        private async Task<IEnumerable<OrderFolder>> GetOrderFolders(string idPhenologicalEvent) {
            var orderFolderQuery = _args.OrderFolder.GetOrderFolders().Where(s => s.PhenologicalEvent.Id.Equals(idPhenologicalEvent));
            return await _args.CommonDb.OrderFolder.TolistAsync(orderFolderQuery);
        }

        private async Task<IEnumerable<string>> GetOrderFoldersIds(string idPhenologicalEvent) {
            var orderFolders = await GetOrderFolders(idPhenologicalEvent);
            return orderFolders.Select(s => s.Id);
        }

        private async Task<IEnumerable<OutPutBarrack>> GetOutputBarracks(string[] ids, string idPhenologicalEvent) {
            var barracks = await GetBarracksFromIds(ids);
            return barracks.Select(async s => {
                var notifications = await GetOutputNotificationEvents(s.Id, idPhenologicalEvent);
                return new OutPutBarrack {
                    Id = s.Id,
                    GeoPoints = s.GeoPoints,
                    CosmosEntityName = s.CosmosEntityName,
                    Hectares = s.Hectares,
                    Name = s.Name,
                    NumberOfPlants = s.NumberOfPlants,
                    PlantingYear = s.PlantingYear,
                    PlotLand = s.PlotLand,
                    Pollinator = s.Pollinator,
                    SeasonId = s.SeasonId,
                    Variety = s.Variety,
                    NotificationEvents = notifications

                };
            }).Select(s => s.Result);
        }

        private async Task<IEnumerable<Barrack>> GetBarracksFromIds(string[] ids) => await ids.SelectElement(_args.Barrack.GetBarrack, "Uno o más cuarteles no fueron encontrados");

        #endregion

    }
}