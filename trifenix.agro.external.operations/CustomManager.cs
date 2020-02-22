using trifenix.agro.db.applicationsReference.common;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;
using trifenix.agro.external.interfaces;
using trifenix.agro.external.interfaces.custom;
using trifenix.agro.external.operations.custom;
using trifenix.agro.external.operations.custom.args;

namespace trifenix.agro.external.operations {
    public class CustomManager : ICustomManager {
        private readonly ITimeStampDbQueries tsRepo;
        private readonly ICommonDbOperations<Barrack> dbBarrackOper;
        private readonly ICommonDbOperations<PhenologicalEvent> dbPhenologicalOper;
        private readonly CommonDbOperations<PhenologicalPreOrder> dbPhenologicalOrder;
        private readonly CommonDbOperations<NotificationEvent> dbNotificationEvent;
        private readonly CommonDbOperations<OrderFolder> dbOrderFolder;
        private readonly string idSeason;
        private readonly IAgroRepository agroRepository;

        public CustomManager(IAgroRepository agroRepository, string idSeason) {
            tsRepo = new TimeStampDbQueries(agroRepository.DbArguments);
            dbBarrackOper = new CommonDbOperations<Barrack>();
            dbPhenologicalOper = new CommonDbOperations<PhenologicalEvent>();
            dbPhenologicalOrder = new CommonDbOperations<PhenologicalPreOrder>();
            dbNotificationEvent = new CommonDbOperations<NotificationEvent>();
            dbOrderFolder = new CommonDbOperations<OrderFolder>();
            this.idSeason = idSeason;
            this.agroRepository = agroRepository;
        }
        

        public IMobileEventCustomElements MobileEvents => new MobileCustomElements(new ArgsMobileCustom { 
            TimeStampDbQuery = tsRepo,
            Barrack = agroRepository.Barracks,
            Phenological = agroRepository.PhenologicalEvents,
            IdSeason = idSeason,
            Variety = agroRepository.Varieties,
            NotificationEvent = agroRepository.NotificationEvents,
            PhenologicalPreOrder = agroRepository.PhenologicalPreOrders,
            OrderFolder = agroRepository.OrderFolder,
            CommonDb = new ArgsCommonMobileDb { 
                Barrack = dbBarrackOper,
                Phenological = dbPhenologicalOper,
                NotificationEvent = dbNotificationEvent,
                PhenologicalPreOrder = dbPhenologicalOrder,
                OrderFolder = dbOrderFolder
            }
        });
    }
}
