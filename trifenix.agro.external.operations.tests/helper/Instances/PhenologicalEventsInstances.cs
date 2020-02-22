using Moq;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class PhenologicalEventsInstances {
        public static Mock<IPhenologicalEventRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<IPhenologicalEventRepository, PhenologicalEvent>(result,
                (s) => s.CreateUpdatePhenologicalEvent(It.IsAny<PhenologicalEvent>()),
                (s) => s.GetPhenologicalEvent(It.IsAny<string>()),
                s => s.GetPhenologicalEvents());

    }
}
