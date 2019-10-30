using Moq;
using System;
using trifenix.agro.common.tests.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.external.operations.tests.helper.staticHelper;

namespace trifenix.agro.external.operations.tests.helper
{
    internal class MoqPhenologicalEventRepo : IMoqRepo<IPhenologicalEventRepository>
    {
        public Mock<IPhenologicalEventRepository> GetMoqRepo(Results result)
        {
            switch (result)
            {
                case Results.Nullables:
                    return MoqPhenologicalEvent.GetPhenologicalEventReturnNull();
                case Results.Empty:
                    return MoqPhenologicalEvent.GetPhenologicalEventReturnEmptyResult();
                case Results.Errors:
                    return MoqPhenologicalEvent.GetPhenologicalEventThrowException();
                case Results.Values:
                    return MoqPhenologicalEvent.GetPhenologicalEventReturnResult();

            }
            throw new Exception("bad parameters");
        }
    }
}