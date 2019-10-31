using Moq;
using trifenix.agro.db.interfaces.agro.ext;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class SicknessInstances
    {
        public static Mock<ISicknessRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<ISicknessRepository, Sickness>(result, (s) =>
                s.CreateUpdateSickness(It.IsAny<Sickness>()),
                (s) => s.GetSickness(It.IsAny<string>()),
                s => s.GetSicknesses());


    }
}
