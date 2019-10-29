using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqBarrack
    {
        private readonly Mock<IBarrackRepository> mockBarrack;

        public MoqBarrack()
        {
            
            mockBarrack = new Mock<IBarrackRepository>();
            mockBarrack.Setup(s => s.CreateUpdateBarrack(It.IsAny<Barrack>())).ReturnsAsync(FakeGenerator.CreateUpdateBarrack());
            mockBarrack.Setup(s => s.GetBarrack(It.IsAny<string>())).ReturnsAsync(FakeGenerator.GetBarrack());
            mockBarrack.Setup(s => s.GetBarracks()).Returns(FakeGenerator.GetBarracks());
        }

        public Mock<IBarrackRepository> GetBarrackRepository => mockBarrack;
    }
}
