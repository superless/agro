using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

using trifenix.agro.common.tests.fakes;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.external.operations.tests.helper.staticHelper
{

    public static class MoqBarrack 
    {
        public static Mock<IBarrackRepository> GetBarrackNullResult() {
            var mockBarrack = new Mock<IBarrackRepository>();
            mockBarrack.Setup(s => s.CreateUpdateBarrack(It.IsAny<Barrack>())).ReturnsAsync((string)null);
            mockBarrack.Setup(s => s.GetBarrack(It.IsAny<string>())).ReturnsAsync((Barrack)null);
            mockBarrack.Setup(s => s.GetBarracks()).Returns((IQueryable<Barrack>)null);
            return mockBarrack;
        }

        public static Mock<IBarrackRepository> GetBarrackThrowException()
        {
            var mockBarrack = new Mock<IBarrackRepository>();
            mockBarrack.Setup(s => s.CreateUpdateBarrack(It.IsAny<Barrack>())).Throws(new Exception("simulated test error"));
            mockBarrack.Setup(s => s.GetBarrack(It.IsAny<string>())).Throws(new Exception("simulated test error"));
            mockBarrack.Setup(s => s.GetBarracks()).Throws(new Exception("simulated test error"));
            return mockBarrack;
        }

        public static Mock<IBarrackRepository> GetBarrackEmptyResult()
        {
            var mockBarrack = new Mock<IBarrackRepository>();
            mockBarrack.Setup(s => s.CreateUpdateBarrack(It.IsAny<Barrack>())).ReturnsAsync(string.Empty);
            mockBarrack.Setup(s => s.GetBarrack(It.IsAny<string>())).ReturnsAsync((Barrack)null);
            mockBarrack.Setup(s => s.GetBarracks()).Returns(new List<Barrack>().AsQueryable());
            return mockBarrack;
        }

        public static Mock<IBarrackRepository> GetBarrackWithResults() {
            var mockBarrack = new Mock<IBarrackRepository>();
            mockBarrack.Setup(s => s.CreateUpdateBarrack(It.IsAny<Barrack>())).ReturnsAsync(FakeGenerator.CreateUpdateBarrack());
            mockBarrack.Setup(s => s.GetBarrack(It.IsAny<string>())).ReturnsAsync(FakeGenerator.GetBarrack());
            mockBarrack.Setup(s => s.GetBarracks()).Returns(FakeGenerator.GetBarracks());
            return mockBarrack;
        }

        
    }
}
