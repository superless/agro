using Moq;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class VarietyInstances {
        public static Mock<IVarietyRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<IVarietyRepository, Variety>(
                result, 
                (s) => s.CreateUpdateVariety(It.IsAny<Variety>()), 
                (s) => s.GetVariety(It.IsAny<string>()), 
                s => s.GetVarieties());

    }
}
