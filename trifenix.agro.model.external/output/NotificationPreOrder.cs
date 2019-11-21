using System;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.model.external.output
{


    public class NotificationCustomPhenologicalResult {

        public long Total { get; set; }

        public OutPutNotificationPreOrder[] Notifications { get; set; }
    }

    public class OutPutNotificationPreOrder : NotificationEvent
    {
        public OutPutPhenologicalPreOrder[] PreOrders { get; set; }

        


    }

    public class OutPutPhenologicalPreOrder
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SeasonId { get; set; }

        public OutPutBarrack[] Barracks { get; set; }


    }

    public class OutPutBarrack : Barrack {

        public OutputNotificationEvent[] NotificationEvents { get; set; }
    }

    public class OutputNotificationEvent {
        public string Id { get; set; }

        public string PicturePath { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }


        



    }



}
