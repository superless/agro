using Moq;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.tests.helper.Moqs;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class IngredientsInstances
    {
        public static Mock<IIngredientRepository> GetInstance(Results result) =>
            MoqGenerator.GetMoqResult<IIngredientRepository, Ingredient>(result, (s) => 
                s.CreateUpdateIngredient(It.IsAny<Ingredient>()), 
                (s) => s.GetIngredient(It.IsAny<string>()), 
                s => s.GetIngredients());

    }
}
