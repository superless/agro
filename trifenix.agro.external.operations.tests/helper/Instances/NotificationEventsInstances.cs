using System;
using trifenix.agro.common.tests.interfaces;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.entities.events;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class NotificationEventsInstances
    {
        public static NotificationEventOperations GetInstance(KindOfInstance kindOfInstance)
        {
            switch (kindOfInstance)
            {
                case KindOfInstance.DefaultReturnValues:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Values).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Values).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);
                case KindOfInstance.DefaultReturnNull:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Nullables).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Values).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Values).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);
                case KindOfInstance.DefaultReturnException:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Errors).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Values).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Values).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);
                case KindOfInstance.DefaultReturnEmpty:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Empty).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Values).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Values).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);
                case KindOfInstance.DefaultReturnValuesNullCommonDb:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Values).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Nullables).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);
                case KindOfInstance.DefaultReturnValuesOkCommonDb:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Values).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Nullables).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);
                case KindOfInstance.DefaultReturnExceptionOnBarracksEmpty:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Nullables).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Nullables).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);
                case KindOfInstance.DefaultReturnExceptionOnPhenologicalEmpty:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Values).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Nullables).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Nullables).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);

                case KindOfInstance.ReturnElementFromCommonDb:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Values).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Values).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);
                case KindOfInstance.NullDb:
                    return new NotificationEventOperations(
                        AgroMoq.NotificationEvent.GetMoqRepo(Results.Nullables).Object,
                        AgroMoq.Barrack.GetMoqRepo(Results.Values).Object,
                        AgroMoq.PhenologicalEvent.GetMoqRepo(Results.Values).Object,
                        AgroMoq.CommonDb<NotificationEvent>().GetMoqRepo(Results.Nullables).Object,
                        AgroMoq.UploadImage.GetMoqRepo(Results.Values).Object);
   

            }
            throw new Exception("bad parameters");
        }
    }

    public enum KindOfInstance
    {
        DefaultReturnValues ,
        DefaultReturnNull,
        DefaultReturnException,
        DefaultReturnEmpty,
        DefaultReturnValuesNullCommonDb,
        DefaultReturnValuesOkCommonDb,
        DefaultReturnExceptionOnBarracksEmpty,
        DefaultReturnExceptionOnPhenologicalEmpty,
        ReturnElementFromCommonDb,
        NullDb,
    }
}
