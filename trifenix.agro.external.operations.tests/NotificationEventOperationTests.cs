using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.tests.helper.Instances;
using trifenix.agro.model.external;
using Xunit;

namespace trifenix.agro.external.operations.tests
{
    public class NotificationEventOperationTests
    {

        #region GetEvent
        [Fact]
        public async Task GetNotificationEventById_ReturnEventSuccess()
        {
            //Arrange

            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnValues);

            //Action
            var result = await repo.GetEvent("5");

            //Assert
            Assert.True(ExtGetDataResult.Success == result.StatusResult);
        }

        [Fact]
        public async Task GetNotificationEventById_ReturnEmptyOnNullResult()
        {
            //Arrange

            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnNull);

            //Action
            var result = await repo.GetEvent("5");
            //Assert
            Assert.True(ExtGetDataResult.EmptyResults == result.StatusResult);
        }

        [Fact]
        public async Task GetNotificationEventById_ReturnEmptyOnErrorDb()
        {
            //Arrange

            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnException);

            //Action
            var result = await repo.GetEvent("5");
            //Assert
            Assert.True(result.GetType() == typeof(ExtGetErrorContainer<NotificationEvent>));
        }
        #endregion

        #region GetNotificationEvets
        [Fact]
        public async Task GetNotificationEvents_ReturnEventsSuccess()
        {
            //Arrange

            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnValues);

            //Action
            var result = await repo.GetEvents();
            //Assert
            Assert.True(ExtGetDataResult.Success == result.StatusResult);
        }

        [Fact]
        public async Task GetNotificationEvents_ReturnEventsEmptyResults()
        {
            //Arrange

            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnEmpty);

            //Action
            var result = await repo.GetEvents();
            //Assert
            Assert.True(ExtGetDataResult.EmptyResults == result.StatusResult);
        }

        [Fact]
        public async Task GetNotificationEvents_ReturnExceptionContainerOnNullList()
        {
            //Arrange

            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnNull);

            //Action
            var result = await repo.GetEvents();

            //Assertt
            Assert.True(result.GetType() == typeof(ExtGetErrorContainer<List<NotificationEvent>>) && ((ExtGetErrorContainer<List<NotificationEvent>>)result).ErrorMessage.Contains("nulo"));
        }
        [Fact]
        public async Task GetNotificationEvents_ReturnExceptionContainerWhenDbException()
        {
            //Arrange            
            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnException);

            //Action
            var result = await repo.GetEvents();

            //Assertt
            Assert.True(result.GetType() == typeof(ExtGetErrorContainer<List<NotificationEvent>>));
        }

        [Fact]
        public async Task GetNotificationEvents_ReturnExceptionWhenListIsNull()
        {
            //Arrange            
            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnNull);

            //Action
            var result = await repo.GetEvents();

            //Assertt
            Assert.True(result.GetType() == typeof(ExtGetErrorContainer<List<NotificationEvent>>));
        }
        #endregion

        #region SaveNewNotificationEvent
        [Theory]
        [InlineData(null, null, null, null, 0F, 0F)]
        [InlineData("", "", "", "", 0F, 0F)]
        public async Task SaveNewNotificationEvent_EmptyAndNullParameters_returnErrorContainer(string idBarrick, string idPhenologicalEvent, string base64, string description, float lat, float lon)
        {
            //Arrange

            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnValuesNullCommonDb);

            //Action
            var result = await repo.SaveNewNotificationEvent(idBarrick, idPhenologicalEvent, base64, description, lat, lon);
            //Assert
            Assert.True(result.GetType() == typeof(ExtPostErrorContainer<string>));
        }


        [Theory]
        [InlineData("a", "s", "d", "f", 0F, 0F)]
        public async Task SaveNewNotificationEvent_differentParameters_returnOk(string idBarrick, string idPhenologicalEvent, string base64, string description, float lat, float lon)
        {
            //Arrange            
            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnValuesOkCommonDb);

            //Action
            var result = await repo.SaveNewNotificationEvent(idBarrick, idPhenologicalEvent, base64, description, lat, lon);
            //Assert
            Assert.True(result.MessageResult == ExtMessageResult.Ok);
        }

        [Theory]
        [InlineData("a", "s", "d", "f", 0F, 0F)]
        public async Task SaveNewNotificationEvent_differentParameters_returnExceptionOnBarracksEmpty(string idBarrick, string idPhenologicalEvent, string base64, string description, float lat, float lon)
        {
            //Arrange            
            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnExceptionOnBarracksEmpty);

            //Action
            var result = await repo.SaveNewNotificationEvent(idBarrick, idPhenologicalEvent, base64, description, lat, lon);
            //Assert
            Assert.True(result.GetType() == typeof(ExtPostErrorContainer<string>));
        }

        [Theory]
        [InlineData("a", "s", "d", "f", 0F, 0F)]
        public async Task SaveNewNotificationEvent_differentParameters_returnExceptionOnPhenologicalEmpty(string idBarrick, string idPhenologicalEvent, string base64, string description, float lat, float lon)
        {
            //Arrange            
            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnExceptionOnPhenologicalEmpty);

            //Action
            var result = await repo.SaveNewNotificationEvent(idBarrick, idPhenologicalEvent, base64, description, lat, lon);
            //Assert
            Assert.True(result.GetType() == typeof(ExtPostErrorContainer<string>));
        }


        [Theory]
        [InlineData("a", "s", "d", "f", 0F, 0F)]
        public async Task SaveNewNotificationEvent_differentParameters_returnExceptionOnElementExists(string idBarrick, string idPhenologicalEvent, string base64, string description, float lat, float lon)
        {
            //Arrange            
            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnValues);

            //Action
            var result = await repo.SaveNewNotificationEvent(idBarrick, idPhenologicalEvent, base64, description, lat, lon);
            //Assert
            Assert.True(result.GetType() == typeof(ExtPostErrorContainer<string>));
        }

        [Theory]
        [InlineData("a", "s", "d", "f", 0F, 0F)]
        public async Task SaveNewNotificationEvent_differentParameters_returnExceptionOnDbException(string idBarrick, string idPhenologicalEvent, string base64, string description, float lat, float lon)
        {
            //Arrange            
            var repo = NotificationEventsInstances.GetInstance(KindOfInstance.DefaultReturnException);

            //Action
            var result = await repo.SaveNewNotificationEvent(idBarrick, idPhenologicalEvent, base64, description, lat, lon);
            //Assert
            Assert.True(result.GetType() == typeof(ExtPostErrorContainer<string>));
        }

        #endregion
    }
}
