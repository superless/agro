using System.Threading.Tasks;
using trifenix.agro.external.operations.tests.helper.Instances;
using trifenix.agro.model.external;
using Xunit;

namespace trifenix.agro.external.operations.tests
{
    public class BarrackOperationsTest
    {
        [Theory]
        [InlineData("inputString")]
        public async Task GetBarrack_Success(string id)
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.DefaultInstance);
            var action = await repo.GetBarrack(id);
            Assert.True(action.StatusResult == ExtGetDataResult.Success);
        }
        [Theory]
        [InlineData("inputString")]
        public async Task GetBarrack_EmptyResult(string id)
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.EmptyResultInstance);
            var action = await repo.GetBarrack(id);
            Assert.True(action.StatusResult == ExtGetDataResult.EmptyResults);
        }
        [Theory]
        [InlineData("inputString")]
        public async Task GetBarrack_Exception(string id)
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.ExceptionInstance);
            var action = await repo.GetBarrack(id);
            Assert.True(action.StatusResult == ExtGetDataResult.Error);
        }

        [Fact]
        public async Task GetBarracks_Success()
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.DefaultInstance);
            var action = await repo.GetBarracks();
            Assert.True(action.StatusResult == ExtGetDataResult.Success);
        }
        [Fact]
        public async Task GetBarracks_EmptyResult()
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.EmptyResultInstance);
            var action = await repo.GetBarracks();
            Assert.True(action.StatusResult == ExtGetDataResult.EmptyResults);
        }
        [Fact]
        public async Task GetBarracks_Exception()
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.ExceptionInstance);
            var action = await repo.GetBarracks();
            Assert.True(action.StatusResult == ExtGetDataResult.Error);
        }

        [Theory]
        [InlineData("ID1", "Cuartel", "Y1", 3F, 2019, "Y2", 100, "Y3")]
        public async Task SaveEditBarrack_Success(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator)
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.SaveNewOrEditBarrack_Success);
            var action = await repo.SaveEditBarrack(id, name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator);
            Assert.True(action.MessageResult == ExtMessageResult.Ok);
        }
        [Theory]
        [InlineData("ID1", "Cuartel", "X1", 3F, 2019, "X2", 100, "X3")]
        public async Task SaveEditBarrack_PlotLandNullInstance(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator)
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.PlotLandNullInstance);
            var action = await repo.SaveEditBarrack(id, name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator);
            Assert.True(action.MessageResult == ExtMessageResult.ElementToEditDoesNotExists);
        }
        [Theory]
        [InlineData("ID1", "Cuartel", "Y1", 3F, 2019, "Y2", 100, "Y3")]
        public async Task SaveEditBarrack_VarietyNullInstance(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator)
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.VarietyNullInstance);
            var action = await repo.SaveEditBarrack(id, name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator);
            Assert.True(action.MessageResult == ExtMessageResult.ElementToEditDoesNotExists);
        }

        [Theory]
        [InlineData("Cuartel", "Y1", 3F, 2019, "Y2", 100, "Y3")]
        public async Task SaveNewBarrack_Success(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator)
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.SaveNewOrEditBarrack_Success);
            var action = await repo.SaveNewBarrack(name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator);
            Assert.True(action.MessageResult == ExtMessageResult.Ok);
        }
        [Theory]
        [InlineData("Cuartel", "X1", 3F, 2019, "X2", 100, "X3")]
        public async Task SaveNewBarrack_PlotLandNullInstance(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator)
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.PlotLandNullInstance);
            var action = await repo.SaveNewBarrack(name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator);
            Assert.True(action.GetType() == typeof(ExtPostErrorContainer<string>));
        }
        [Theory]
        [InlineData("Cuartel", "Y1", 3F, 2019, "Y2", 100, "Y3")]
        public async Task SaveNewBarrack_VarietyNullInstance(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator)
        {
            var repo = BarrackInstances.GetBarrackOperations(BarrackEnumIntances.VarietyNullInstance);
            var action = await repo.SaveNewBarrack(name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator);
            Assert.True(action.GetType() == typeof(ExtPostErrorContainer<string>));
        }
    }
}
