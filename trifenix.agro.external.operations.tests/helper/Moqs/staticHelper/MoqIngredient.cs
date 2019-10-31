using Moq;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.external.operations.tests.helper.Moqs.staticHelper
{
    public static class MoqIngredient
    {
        public static Mock<IIngredientRepository> GetBarrackWithResults()
        {
            var mockBarrack = new Mock<IIngredientRepository>();
            mockBarrack.Setup(s => s.CreateUpdateIngredient(It.IsAny<Ingredient>())).ReturnsAsync(FakeGenerator.CreateString());
            mockBarrack.Setup(s => s.GetIngredient(It.IsAny<string>())).ReturnsAsync(FakeGenerator.GetElement<Ingredient>());
            mockBarrack.Setup(s => s.GetIngredients()).Returns(FakeGenerator.GetElements<Ingredient>());
            return mockBarrack;
        }
    }
}
