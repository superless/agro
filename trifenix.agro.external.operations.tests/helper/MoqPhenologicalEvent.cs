using Moq;
using System;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqPhenologicalEvent
    {
        private readonly Mock<IPhenologicalEventRepository> mockPhenological;

        public MoqPhenologicalEvent()
        {
            mockPhenological = new Mock<IPhenologicalEventRepository>();
            mockPhenological.Setup(s => s.CreateUpdatePhenologicalEvent(It.IsAny<PhenologicalEvent>())).ReturnsAsync(FakeGenerator.CreateUpdatePhenologicalEvent());
            mockPhenological.Setup(s => s.GetPhenologicalEvent(It.IsAny<string>())).ReturnsAsync(FakeGenerator.GetPhenologicalEvent());
            mockPhenological.Setup(s => s.GetPhenologicalEvents()).Returns(FakeGenerator.GetPhenologicalEvents());
        }

        public Mock<IPhenologicalEventRepository> GetPhenologicalEventRepository => mockPhenological;



    }
}
