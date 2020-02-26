using Moq;
using System.Collections.Generic;
using trifenix.agro.search.interfaces;
using trifenix.agro.search.model;
using trifenix.agro.search.operations;

namespace trifenix.agro.external.operations.tests.helper.Instances {

    public static class AgroSearchInstances {
        public static Mock<IAgroSearch> GetInstance(){
            var mockWeatherApi = new Mock<IAgroSearch>();
            mockWeatherApi.Setup(s => s.AddElements(It.IsAny<List<EntitySearch>>()));
            mockWeatherApi.Setup(s => s.GetPaginatedEntities(It.IsAny<Parameters>())).Returns(new EntitiesSearchContainer());
            mockWeatherApi.Setup(s => s.DeleteElements(It.IsAny<List<EntitySearch>>()));
            return mockWeatherApi;
        }
    }

}