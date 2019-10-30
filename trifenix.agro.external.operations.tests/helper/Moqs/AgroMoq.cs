
using trifenix.agro.common.tests.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations.tests.helper
{
    public static class AgroMoq
    {
        public static IMoqRepo<IBarrackRepository> Barrack => new MoqBarrackRepo();

        public static IMoqRepo<ICommonDbOperations<T>> CommonDb<T>() where T:class {
            return new MoqCommonDbRepo<T>();
        }

        public static IMoqRepo<INotificationEventRepository> NotificationEvent => new MoqNotificationEventRepo();

        public static IMoqRepo<IUploadImage> UploadImage => new MoqUploadImageRepo();

        public static IMoqRepo<IPhenologicalEventRepository> PhenologicalEvent => new MoqPhenologicalEventRepo();
    }
}
