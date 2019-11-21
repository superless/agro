using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.db.model.agro.enums;
using trifenix.agro.external.operations.tests.helper.Instances;
using trifenix.agro.model.external;
using trifenix.agro.model.external.Input;
using Xunit;

namespace trifenix.agro.external.operations.tests
{
    public class ProductOperationTest
    {
        [Theory]
        [InlineData("atlas43", "gfdhgfghkh","marcaX", null, MeasureType.KL,100, KindOfProductContainer.Bottle)]
        public async Task SaveProduct_allParameters_success(string commercialName, string idActiveIngredient, string brand, DosesInput[] doses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct) {

            var repo = ProductInstances.GetProductOperations(ProductEnumInstances.DefaultInstance);

            var action = await repo.CreateProduct(commercialName, idActiveIngredient, brand, doses, measureType, quantity, kindOfProduct);

            Assert.True(action.MessageResult == ExtMessageResult.Ok);
        }


        [Fact]
        public async Task SaveProduct_dosesInParameters_saveSuccessfull() {
            var doses = FakeGenerator.GetElements<DosesInput>().ToArray();
            var repo = ProductInstances.GetProductOperations(ProductEnumInstances.DefaultInstance);

            var action = await repo.CreateProduct("atlas43", "gfdhgfghkh", "marcaX", doses, MeasureType.KL, 100, KindOfProductContainer.Bottle);

            Assert.True(action.MessageResult == ExtMessageResult.Ok);
        }

        [Fact]
        public async Task SaveProduct_dosesInParameters_NullFromDb()
        {
            var doses = FakeGenerator.GetElements<DosesInput>().ToArray();
            var repo = ProductInstances.GetProductOperations(ProductEnumInstances.DefaultInstanceNullIds);

            var action = await repo.CreateProduct("atlas43", "gfdhgfghkh", "marcaX", doses, MeasureType.KL, 100, KindOfProductContainer.Bottle);

            Assert.True(action.GetType() == typeof(ExtPostErrorContainer<string>));
        }

        [Theory]
        [InlineData(null, "gfdhgfghkh", "marcaX", null, MeasureType.KL, 100, KindOfProductContainer.Bottle)]
        [InlineData("atlas43", null, "marcaX", null, MeasureType.KL, 100, KindOfProductContainer.Bottle)]
        [InlineData("atlas43", "gfdhgfghkh", null, null, MeasureType.KL, 100, KindOfProductContainer.Bottle)]
        [InlineData(null, null, null, null, MeasureType.KL, 100, KindOfProductContainer.Bottle)]
        public async Task SaveProduct_nullparameters_error(string commercialName, string idActiveIngredient, string brand, DosesInput[] idDoses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct)
        {

            var repo = ProductInstances.GetProductOperations(ProductEnumInstances.DefaultInstance);

            var action = await repo.CreateProduct(commercialName, idActiveIngredient, brand, idDoses, measureType, quantity, kindOfProduct);

            Assert.True(action.MessageResult == ExtMessageResult.Error);
        }

        [Theory]
         [InlineData("atlas43", "gfdhgfghkh","marcaX", null, MeasureType.KL,100, KindOfProductContainer.Bottle)]
        public async Task SaveProduct_nullparameters_NoIngredienteObDbGetExceptionContainer(string commercialName, string idActiveIngredient, string brand, DosesInput[] idDoses, MeasureType measureType, int quantity, KindOfProductContainer kindOfProduct)
        {

            var repo = ProductInstances.GetProductOperations(ProductEnumInstances.InstanceNoIngredientOnDb);

            var action = await repo.CreateProduct(commercialName, idActiveIngredient, brand, idDoses, measureType, quantity, kindOfProduct);

            Assert.True(action.MessageResult == ExtMessageResult.ElementToEditDoesNotExists);
        }
    }
}
