using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.agro.orders;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.db.model;

namespace trifenix.agro.external.operations.custom.args
{
    public class ArgsMobileCustom
    {
        public ITimeStampDbQueries TimeStampDbQuery { get; set; }
        public IBarrackRepository Barrack { get; set; }

        public IPhenologicalEventRepository Phenological { get; set; }

        public IVarietyRepository Variety { get; set; }


        public string IdSeason { get; set; }

        public ArgsCommonMobileDb CommonDb { get; set; }


        //list of custom notifications
        public IPhenologicalPreOrderRepository PhenologicalPreOrder { get; set; }

        public INotificationEventRepository NotificationEvent { get; set; }


        public IOrderFolderRepository OrderFolder { get; set; }






    }

    public class ArgsCommonMobileDb {
        public ICommonDbOperations<Barrack> Barrack { get; set; }

        public ICommonDbOperations<PhenologicalEvent> Phenological { get; set; }

        public ICommonDbOperations<PhenologicalPreOrder> PhenologicalPreOrder { get; set; }

        public ICommonDbOperations<NotificationEvent> NotificationEvent { get; set; }
        
        public ICommonDbOperations<OrderFolder> OrderFolder { get; set; }








    }
}
