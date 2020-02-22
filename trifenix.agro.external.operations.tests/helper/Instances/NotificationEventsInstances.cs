using Moq;
using System;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.model;
using trifenix.agro.external.operations.entities.events;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances {

    public static class NotificationEventsInstances {

        public static Mock<INotificationEventRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<INotificationEventRepository, NotificationEvent>(result, 
                (s) => s.CreateUpdateNotificationEvent(It.IsAny<NotificationEvent>()), 
                (s) => s.GetNotificationEvent(It.IsAny<string>()), 
                s => s.GetNotificationEvents());

        public static NotificationEventOperations GetInstance(KindOfInstance kindOfInstance) {
            switch (kindOfInstance) {
                case KindOfInstance.DefaultReturnValues:
                    return new NotificationEventOperations(
                        GetInstance(Results.Values).Object,
                        BarrackInstances<Barrack>.GetInstance(Results.Values).Object,
                        PhenologicalEventsInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<NotificationEvent>.GetInstance(Results.Values).Object,
                        UploadImageInstances.GetInstance(Results.Values).Object,
                        null,
                        null
                    );
                case KindOfInstance.DefaultReturnNull:
                    return new NotificationEventOperations(
                        GetInstance(Results.Nullables).Object,
                        BarrackInstances<Barrack>.GetInstance(Results.Values).Object,
                        PhenologicalEventsInstances.GetInstance(Results.Values).Object,
                        CommonDbInstances<NotificationEvent>.GetInstance(Results.Values).Object,
                        UploadImageInstances.GetInstance(Results.Values).Object,
                        null,
                        null
                    );
                case KindOfInstance.DefaultReturnException:
                    return new NotificationEventOperations(
                       GetInstance(Results.Errors).Object,
                       BarrackInstances<Barrack>.GetInstance(Results.Values).Object,
                       PhenologicalEventsInstances.GetInstance(Results.Values).Object,
                       CommonDbInstances<NotificationEvent>.GetInstance(Results.Values).Object,
                       UploadImageInstances.GetInstance(Results.Values).Object,
                       null,
                       null
                   );

                case KindOfInstance.DefaultReturnEmpty:
                    return new NotificationEventOperations(
                       GetInstance(Results.Empty).Object,
                       BarrackInstances<Barrack>.GetInstance(Results.Values).Object,
                       PhenologicalEventsInstances.GetInstance(Results.Values).Object,
                       CommonDbInstances<NotificationEvent>.GetInstance(Results.Values).Object,
                       UploadImageInstances.GetInstance(Results.Values).Object,
                       null,
                       null
                   );

                case KindOfInstance.DefaultReturnValuesNullCommonDb:
                    return new NotificationEventOperations(
                       GetInstance(Results.Values).Object,
                       BarrackInstances<Barrack>.GetInstance(Results.Values).Object,
                       PhenologicalEventsInstances.GetInstance(Results.Values).Object,
                       CommonDbInstances<NotificationEvent>.GetInstance(Results.Nullables).Object,
                       UploadImageInstances.GetInstance(Results.Values).Object,
                       null,
                       null
                   );
                case KindOfInstance.DefaultReturnValuesOkCommonDb:
                    return new NotificationEventOperations(
                       GetInstance(Results.Values).Object,
                       BarrackInstances<Barrack>.GetInstance(Results.Values).Object,
                       PhenologicalEventsInstances.GetInstance(Results.Values).Object,
                       CommonDbInstances<NotificationEvent>.GetInstance(Results.Nullables).Object,
                       UploadImageInstances.GetInstance(Results.Values).Object,
                       GraphApiInstances.GetInstance().Object,
                       WeatherApiInstances.GetInstance().Object
                   );
                case KindOfInstance.DefaultReturnExceptionOnBarracksEmpty:
                    return new NotificationEventOperations(
                       GetInstance(Results.Values).Object,
                       BarrackInstances<Barrack>.GetInstance(Results.Nullables).Object,
                       PhenologicalEventsInstances.GetInstance(Results.Values).Object,
                       CommonDbInstances<NotificationEvent>.GetInstance(Results.Values).Object,
                       UploadImageInstances.GetInstance(Results.Values).Object,
                       null,
                       null
                   );
                case KindOfInstance.DefaultReturnExceptionOnPhenologicalEmpty:
                    return new NotificationEventOperations(
                       GetInstance(Results.Values).Object,
                       BarrackInstances<Barrack>.GetInstance(Results.Values).Object,
                       PhenologicalEventsInstances.GetInstance(Results.Nullables).Object,
                       CommonDbInstances<NotificationEvent>.GetInstance(Results.Values).Object,
                       UploadImageInstances.GetInstance(Results.Values).Object,
                       null,
                       null
                   );
                case KindOfInstance.NullDb:
                    return new NotificationEventOperations(
                       GetInstance(Results.Values).Object,
                       BarrackInstances<Barrack>.GetInstance(Results.Nullables).Object,
                       PhenologicalEventsInstances.GetInstance(Results.Nullables).Object,
                       CommonDbInstances<NotificationEvent>.GetInstance(Results.Nullables).Object,
                       UploadImageInstances.GetInstance(Results.Values).Object,
                       null,
                       null
                   );
            }
            throw new Exception("bad parameters");
        }
    }

    public enum KindOfInstance {
        DefaultReturnValues ,
        DefaultReturnNull,
        DefaultReturnException,
        DefaultReturnEmpty,
        DefaultReturnValuesNullCommonDb,
        DefaultReturnValuesOkCommonDb,
        DefaultReturnExceptionOnBarracksEmpty,
        DefaultReturnExceptionOnPhenologicalEmpty,        
        NullDb,
    }
}