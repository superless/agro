using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.local;
using trifenix.agro.db.model.agro.orders;
using trifenix.agro.external.interfaces.entities.orders;
using trifenix.agro.external.operations.common;
using trifenix.agro.external.operations.entities.orders.args;
using trifenix.agro.external.operations.helper;
using trifenix.agro.microsoftgraph.operations;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using trifenix.agro.model.external.output;
using trifenix.agro.util;

namespace trifenix.agro.external.operations.entities.orders
{
    public class ApplicationOrderOperations : IApplicationOrderOperations {
        private readonly ApplicationOrderArgs _args;
        public ApplicationOrderOperations(ApplicationOrderArgs args)
        {
            _args = args;
        }

        private OutPutApplicationOrder GetOutputOrder(ApplicationOrder appOrder) {
            return new OutPutApplicationOrder
            {
                Id = appOrder.Id,
                Wetting = appOrder.Wetting,
                Name = appOrder.Name,
                InitDate = appOrder.InitDate,
                EndDate = appOrder.EndDate,
                SeasonId = appOrder.SeasonId,
                ApplicationInOrders = appOrder.ApplicationInOrders.Select(async s =>
                {


                    return new OutPutApplicationInOrder
                    {
                        Doses = s.Doses,
                        Product = await _args.Product.GetProduct(s.ProductId),
                        ProductId = s.ProductId,
                        QuantityByHectare = s.QuantityByHectare
                    };
                }).Select(s => s.Result).ToList(),
                PhenologicalPreOrders = appOrder.PhenologicalPreOrders,
                Barracks = appOrder.Barracks.Select(async s =>
                {

                    var events = await s.EventsId.SelectElement(_args.Notifications.GetNotificationEvent, "identicadores de evento no encontrados");


                    return new OutputBarrackInstance
                    {
                        Barrack = s.Barrack,
                        EventsId = s.EventsId,
                        Events = events.Select(a => new OutputOrderNotificationEvent
                        {
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

        public async Task<ExtGetContainer<OutPutApplicationOrder>> GetApplicationOrder(string id)
        {
            try
            {
                var appOrder = await _args.ApplicationOrder.GetApplicationOrder(id);

                var newAppOrder = GetOutputOrder(appOrder);


                return OperationHelper.GetElement(newAppOrder);
            }
            catch (Exception e)
            {

                return OperationHelper.GetException<OutPutApplicationOrder>(e, e.Message);
            }
        }

        public async Task<ExtGetContainer<List<OutPutApplicationOrder>>> GetApplicationOrders()
        {
            try
            {
                var applicationOrderQuery = _args.ApplicationOrder.GetApplicationOrders();
                var applicationOrders = await _args.CommonDb.ApplicationOrder.TolistAsync(applicationOrderQuery);
                var outputOrders = applicationOrders.Select(GetOutputOrder).ToList();

                return OperationHelper.GetElements(outputOrders);

            }
            catch (Exception e)
            {

                return OperationHelper.GetException<List<OutPutApplicationOrder>>(e, e.Message); 
            }
        }

        public async Task<ExtPostContainer<OutPutApplicationOrder>> SaveEditApplicationOrder(string id, ApplicationOrderInput input)
        {
            var modifier = await _args.GraphApi.GetUserFromToken();
            var userActivity = new UserActivity(DateTime.Now, modifier);
            var order = await _args.ApplicationOrder.GetApplicationOrder(id);
            var appNewOrder = await GetApplicationOrder(id, input);
            var result = await OperationHelper.EditElement(_args.CommonDb.ApplicationOrder, _args.ApplicationOrder.GetApplicationOrders(),
                id,
                order,
                s => {
                    appNewOrder.Creator = s.Creator;
                    appNewOrder.ModifyBy = s.ModifyBy;
                    appNewOrder.ModifyBy.Add(userActivity);
                    return appNewOrder;
                },
                _args.ApplicationOrder.CreateUpdate,
                $"No existe orden con id {id}",
                s => s.Name.Equals(input.Name) && input.Name != order.Name,
                $"Ya existe orden de aplicacion con nombre : {input.Name}"
            );
            return new ExtPostContainer<OutPutApplicationOrder>
            {
                IdRelated = result.IdRelated,
                Message = result.Message,
                MessageResult = result.MessageResult,
                Result = GetOutputOrder(appNewOrder)
            };
        }

        private async Task<ApplicationOrder> GetApplicationOrder(string id, ApplicationOrderInput input) {
            var varietyIds = input.Applications.Any(s => s.Doses != null)? input.Applications.Where(s => s.Doses != null).SelectMany(s => s.Doses.IdVarieties).Distinct() : new List<string>();
            var targetIds = input.Applications.Any(s => s.Doses != null) ? input.Applications.Where(s => s.Doses != null).SelectMany(s => s.Doses.idsApplicationTarget).Distinct(): new List<string>();
            var speciesIds = input.Applications.Any(s => s.Doses != null) ? input.Applications.Where(s => s.Doses != null).SelectMany(s => s.Doses.IdSpecies).Distinct() : new List<string>();
            var certifiedEntitiesIds = input.Applications.Any(s=>s.Doses!=null)? input.Applications.Where(s=>s.Doses!=null).SelectMany(s => s.Doses.WaitingHarvest.Select(a => a.IdCertifiedEntity)).Distinct(): new List<string>();
            var barracksInstances = await GetBarracksIntance(input.BarracksInput);
            var applications = GetApplicationInOrder(input.Applications);
            var phenologicalPreOrders = input.PreOrdersId == null || !input.PreOrdersId.Any() ? new List<PhenologicalPreOrder>() :
                           await input.PreOrdersId.SelectElement(_args.PreOrder.GetPhenologicalPreOrder, "Existen identificadores de preordenes que no fueron encontrados");
            var creator = await _args.GraphApi.GetUserFromToken();
            var userActivity = new UserActivity(DateTime.Now, creator);
            return new ApplicationOrder
            {
                Id = id,
                IdsCertifiedEntities = certifiedEntitiesIds?.ToList(),
                IdsSpecies = speciesIds?.ToList(),
                Barracks = barracksInstances,
                IdsTargets = targetIds?.ToList(),
                IdVarieties = varietyIds?.ToList(),
                SeasonId = _args.SeasonId,
                Name = input.Name,
                InitDate = input.InitDate,
                EndDate = input.EndDate,
                Wetting = input.Wetting,
                ApplicationInOrders = applications,
                Creator = userActivity,
                PhenologicalPreOrders = phenologicalPreOrders
            };
        }

        public async Task<ExtPostContainer<string>> SaveNewApplicationOrder(ApplicationOrderInput input)
        {
            return await OperationHelper.CreateElement(_args.CommonDb.ApplicationOrder, _args.ApplicationOrder.GetApplicationOrders(),
                       async s => await _args.ApplicationOrder.CreateUpdate(await GetApplicationOrder(s, input)),
                       s => s.Name.Equals(input.Name),
                       $"Ya existe orden de aplicacion con nombre: {input.Name}"
                   ) ;
        }

        private List<ApplicationsInOrder> GetApplicationInOrder(ApplicationInOrderInput[] appInOrder) {

            

            return appInOrder.Select(async s =>
            {
                if (s.Doses == null)
                {
                    return new ApplicationsInOrder
                    {
                        ProductId = s.ProductId,
                        QuantityByHectare = s.QuantityByHectare

                    };
                }

                var dose = await GetDose(s.Doses);
                return new ApplicationsInOrder
                {
                    ProductId = s.ProductId,
                    QuantityByHectare = s.QuantityByHectare,
                    Doses = dose

                };


            }).Select(s => s.Result).ToList();
        }

        private async Task<Doses> GetDose(DosesInput input)
        {

            var doses = new List<DosesInput> { input };
            var idCerts = input.WaitingHarvest.Select(s => s.IdCertifiedEntity).Distinct();


            var dosesResult= await ModelCommonOperations.GetDoses(_args.DosesArgs.Variety, _args.DosesArgs.Target,
                _args.DosesArgs.Specie, _args.DosesArgs.CertifiedEntity, doses.ToArray(), input.IdVarieties, input.idsApplicationTarget, input.IdSpecies, idCerts, _args.SeasonId);

            return dosesResult.First();

        }

        private async Task<List<BarrackOrderInstance>> GetBarracksIntance(BarrackEventInput[] barracksInput) {
           
            var barracks = await barracksInput.Select(s => s.IdBarrack).SelectElement(_args.Barracks.GetBarrack, "Uno de los identificadores de cuartel no fue encontrado");

            return barracksInput.Select(s =>
            {
                return new BarrackOrderInstance
                {
                    Barrack = barracks.First(a=>a.Id.Equals(s.IdBarrack)),
                    EventsId = s.EventsId?.ToList()
                };
            }).ToList();

        }
    }
}
