using Moq;
using trifenix.agro.db.model.agro;
using trifenix.agro.microsoftgraph.interfaces;

namespace trifenix.agro.external.operations.tests.helper.Instances
{

    public static class GraphApiInstances
    {
        public static Mock<IGraphApi> GetInstance(){
            var mockWeatherApi = new Mock<IGraphApi>();
            mockWeatherApi.Setup(s => s.GetUserFromToken()).ReturnsAsync(It.IsAny<UserApplicator>());
            mockWeatherApi.Setup(s => s.GetObjectIdFromEmail(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>());
            mockWeatherApi.Setup(s => s.CreateUserIntoActiveDirectory()).ReturnsAsync(It.IsAny<string>());
            return mockWeatherApi;
        }
    }
}