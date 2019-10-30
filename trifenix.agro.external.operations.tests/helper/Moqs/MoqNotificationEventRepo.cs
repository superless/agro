using Moq;
using System;
using trifenix.agro.common.tests.interfaces;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.external.operations.tests.helper.staticHelper;

namespace trifenix.agro.external.operations.tests.helper
{
    internal class MoqNotificationEventRepo : IMoqRepo<INotificationEventRepository>
    {
        public Mock<INotificationEventRepository> GetMoqRepo(Results result)
        {
            switch (result)
            {
                case Results.Nullables:
                    return MoqNotificationEvent.GetNotificationEventReturnNullResults();
                case Results.Empty:
                    return MoqNotificationEvent.GetNotificationEventReturnEmpty();
                case Results.Errors:
                    return MoqNotificationEvent.GetNotificationEventThrowException();
                case Results.Values:
                    return MoqNotificationEvent.GetNotificationEventReturnResults();
            }
            throw new Exception("bad parameters");
        }
    }
}