using Moq;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class RootstockInstances{
        public static Mock<IRootstockRepository> GetInstance(Results result) =>
           MoqGenerator.GetMoqResult<IRootstockRepository, Rootstock>(
               result,
               (s) => s.CreateUpdateRootstock(It.IsAny<Rootstock>()),
               (s) => s.GetRootstock(It.IsAny<string>()),
               s => s.GetRootstocks());
    }
}
