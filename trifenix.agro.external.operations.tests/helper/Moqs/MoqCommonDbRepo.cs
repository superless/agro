
using Moq;
using System;
using trifenix.agro.common.tests.interfaces;
using trifenix.agro.db.interfaces.common;
using trifenix.agro.external.operations.tests.helper.staticHelper;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqCommonDbRepo<T> : IMoqRepo<ICommonDbOperations<T>> where T:class
    {
        public Mock<ICommonDbOperations<T>> GetMoqRepo(Results result) 
        {
            switch (result)
            {
                case Results.Nullables:
                    return MoqCommonDb.GetDbOperationsReturnNull<T>();
                case Results.Empty:
                    return MoqCommonDb.GetDbOperationsReturnEmpty<T>();
                case Results.Errors:
                    return MoqCommonDb.GetDbOperationsThrowException<T>();
                case Results.Values:
                    return MoqCommonDb.GetDbOperationsReturnResult<T>();

            }
            throw new Exception("bad parameters");
        }
    }
}
