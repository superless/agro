using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.external.operations.tests.helper.staticHelper
{
    public static class MoqPhenologicalEvent
    {
        
        public static Mock<IPhenologicalEventRepository> GetPhenologicalEventReturnResult() {
            var mockPhenological = new Mock<IPhenologicalEventRepository>();
            mockPhenological.Setup(s => s.CreateUpdatePhenologicalEvent(It.IsAny<PhenologicalEvent>())).ReturnsAsync(FakeGenerator.CreateUpdatePhenologicalEvent());
            mockPhenological.Setup(s => s.GetPhenologicalEvent(It.IsAny<string>())).ReturnsAsync(FakeGenerator.GetPhenologicalEvent());
            mockPhenological.Setup(s => s.GetPhenologicalEvents()).Returns(FakeGenerator.GetPhenologicalEvents());
            return mockPhenological;
        }

        public static Mock<IPhenologicalEventRepository> GetPhenologicalEventReturnNull()
        {
            var mockPhenological = new Mock<IPhenologicalEventRepository>();
            mockPhenological.Setup(s => s.CreateUpdatePhenologicalEvent(It.IsAny<PhenologicalEvent>())).ReturnsAsync((string)null);
            mockPhenological.Setup(s => s.GetPhenologicalEvent(It.IsAny<string>())).ReturnsAsync((PhenologicalEvent)null);
            mockPhenological.Setup(s => s.GetPhenologicalEvents()).Returns((IQueryable<PhenologicalEvent>)null);
            return mockPhenological;
        }

        public static Mock<IPhenologicalEventRepository> GetPhenologicalEventReturnEmptyResult()
        {
            var mockPhenological = new Mock<IPhenologicalEventRepository>();
            mockPhenological.Setup(s => s.CreateUpdatePhenologicalEvent(It.IsAny<PhenologicalEvent>())).ReturnsAsync(string.Empty);
            mockPhenological.Setup(s => s.GetPhenologicalEvent(It.IsAny<string>())).ReturnsAsync((PhenologicalEvent)null);
            mockPhenological.Setup(s => s.GetPhenologicalEvents()).Returns(new List<PhenologicalEvent>().AsQueryable());
            return mockPhenological;
        }

        public static Mock<IPhenologicalEventRepository> GetPhenologicalEventThrowException()
        {
            var mockPhenological = new Mock<IPhenologicalEventRepository>();
            mockPhenological.Setup(s => s.CreateUpdatePhenologicalEvent(It.IsAny<PhenologicalEvent>())).Throws(new Exception("simulated test error"));
            mockPhenological.Setup(s => s.GetPhenologicalEvent(It.IsAny<string>())).Throws(new Exception("simulated test error"));
            mockPhenological.Setup(s => s.GetPhenologicalEvents()).Throws(new Exception("simulated test error"));
            return mockPhenological;
        }





    }
}
