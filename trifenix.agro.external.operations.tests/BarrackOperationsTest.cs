using Moq;
using System.Threading.Tasks;
using trifenix.agro.db.model;
using trifenix.agro.external.operations.tests.helper.Instances;
using trifenix.agro.model.external;
using Xunit;

namespace trifenix.agro.external.operations.tests {
    public class BarrackOperationsTest {

        [Theory]
        [InlineData("inputString")]
        public async Task GetBarrack_Success(string id) {
            var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.DefaultInstance);
            var action = await repo.GetBarrack(id);
            Assert.True(action.StatusResult == ExtGetDataResult.Success);
        }

        [Theory]
        [InlineData("inputString")]
        public async Task GetBarrack_EmptyResult(string id) {
            var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.EmptyResultInstance);
            var action = await repo.GetBarrack(id);
            Assert.True(action.StatusResult == ExtGetDataResult.EmptyResults);
        }

        [Theory]
        [InlineData("inputString")]
        public async Task GetBarrack_Exception(string id) {
            var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.ExceptionInstance);
            var action = await repo.GetBarrack(id);
            Assert.True(action.StatusResult == ExtGetDataResult.Error);
        }

        //[Fact]
        //public void GetBarracks_Success() {
        //    var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.DefaultInstance);
        //    var action = repo.GetPaginatedBarracks(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
        //    Assert.True(action.StatusResult == ExtGetDataResult.Success);
        //}

        //[Fact]
        //public void GetBarracks_EmptyResult() {
        //    var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.EmptyResultInstance);
        //    var action = repo.GetPaginatedBarracks(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
        //    Assert.True(action.StatusResult == ExtGetDataResult.EmptyResults);
        //}

        //[Fact]
        //public void GetBarracks_Exception() {
        //    var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.ExceptionInstance);
        //    var action = repo.GetPaginatedBarracks(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>());
        //    Assert.True(action.StatusResult == ExtGetDataResult.Error);
        //}

        [Theory]
        [InlineData("ID1", "Cuartel", "Y1", 3F, 2019, "Y2", 100, "Y3", "X")]
        public async Task SaveEditBarrack_Success(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock) {
            var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.SaveNewOrEditBarrack_Success);
            var action = await repo.SaveEditBarrack(id, name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator, idRootstock);
            Assert.True(action.MessageResult == ExtMessageResult.Ok);
        }

        [Theory]
        [InlineData("ID1", "Cuartel", "X1", 3F, 2019, "X2", 100, "X3", "X")]
        public async Task SaveEditBarrack_PlotLandNullInstance(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock) {
            var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.PlotLandNullInstance);
            var action = await repo.SaveEditBarrack(id, name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator, idRootstock);
            Assert.True(action.MessageResult == ExtMessageResult.ElementToEditDoesNotExists);
        }

        [Theory]
        [InlineData("ID1", "Cuartel", "Y1", 3F, 2019, "Y2", 100, "Y3", "X")]
        public async Task SaveEditBarrack_VarietyNullInstance(string id, string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock) {
            var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.VarietyNullInstance);
            var action = await repo.SaveEditBarrack(id, name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator, idRootstock);
            Assert.True(action.MessageResult == ExtMessageResult.ElementToEditDoesNotExists);
        }

        [Theory]
        [InlineData("Cuartel", "Y1", 3F, 2019, "Y2", 100, "Y3", "X")]
        public async Task SaveNewBarrack_Success(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock) {
            var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.SaveNewOrEditBarrack_Success);
            var action = await repo.SaveNewBarrack(name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator, idRootstock);
            Assert.True(action.MessageResult == ExtMessageResult.Ok);
        }

        [Theory]
        [InlineData("Cuartel", "X1", 3F, 2019, "X2", 100, "X3", "X")]
        public async Task SaveNewBarrack_PlotLandNullInstance(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock) {
            var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.PlotLandNullInstance);
            var action = await repo.SaveNewBarrack(name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator, idRootstock);
            Assert.True(action.GetType() == typeof(ExtPostErrorContainer<string>));
        }

        [Theory]
        [InlineData("Cuartel", "Y1", 3F, 2019, "Y2", 100, "Y3","X")]
        public async Task SaveNewBarrack_VarietyNullInstance(string name, string idPlotLand, float hectares, int plantingYear, string idVariety, int numberOfPlants, string idPollinator, string idRootstock) {
            var repo = BarrackInstances<Barrack>.GetBarrackOperations(BarrackEnumInstances.VarietyNullInstance);
            var action = await repo.SaveNewBarrack(name, idPlotLand, hectares, plantingYear, idVariety, numberOfPlants, idPollinator, idRootstock);
            Assert.True(action.GetType() == typeof(ExtPostErrorContainer<string>));
        }

    }
}
