using Moq;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class BarrackInstances {
        public static Mock<IBarrackRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<IBarrackRepository, Barrack>(result, 
                (s) => s.CreateUpdateBarrack(It.IsAny<Barrack>()), 
                (s) => s.GetBarrack(It.IsAny<string>()), 
                s => s.GetBarracks());
    }
}
