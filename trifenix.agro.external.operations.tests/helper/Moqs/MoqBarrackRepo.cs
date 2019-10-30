using Moq;
using System;
using trifenix.agro.common.tests.interfaces;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.external.operations.tests.helper.staticHelper;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqBarrackRepo : IMoqRepo<IBarrackRepository>
    {
        public Mock<IBarrackRepository> GetMoqRepo(Results result)
        {
            switch (result)
            {
                case Results.Nullables:
                    return MoqBarrack.GetBarrackNullResult();
                case Results.Empty:
                    return MoqBarrack.GetBarrackEmptyResult();
                case Results.Errors:
                    return MoqBarrack.GetBarrackThrowException();
                case Results.Values:
                    return MoqBarrack.GetBarrackWithResults();
                
            }
            throw new Exception("bad input parameters");
        }
    }
}
