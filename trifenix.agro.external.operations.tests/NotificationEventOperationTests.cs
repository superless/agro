using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.external.operations.entities.events;
using trifenix.agro.external.operations.tests.helper;
using trifenix.agro.model.external;
using Xunit;

namespace trifenix.agro.external.operations.tests
{
    public class NotificationEventOperationTests
    {

       
        [Fact]
        public async Task GetNotificationEventById_ReturnEventSuccess()
        {
            //Arrange
            
            var repo = new NotificationEventOperations(MoqHelper.NotificationEvent.GetNotificationEventRepository.Object,
                MoqHelper.Barrack.GetBarrackRepository.Object,
                MoqHelper.PhenologicalEvent.GetPhenologicalEventRepository.Object, 
                MoqHelper.CommonDb.GetDbOperations<NotificationEvent>().Object, 
                MoqHelper.UpImage.GetUploadImage.Object);
            //Action
            var result = await repo.GetEvent("5");
            //Assert
            Assert.True(ExtGetDataResult.Success == result.StatusResult);
        }

        [Fact]
        public async Task GetNotificationEventById_ReturnEmptyOnNullResult()
        {
            //Arrange
            
            var repo = new NotificationEventOperations(
                MoqHelper.NotificationEvent.GetNotificationEventRepositoryReturnNullOnGetEvent().Object, 
                MoqHelper.Barrack.GetBarrackRepository.Object, 
                MoqHelper.PhenologicalEvent.GetPhenologicalEventRepository.Object, 
                MoqHelper.CommonDb.GetDbOperations<NotificationEvent>().Object, 
                MoqHelper.UpImage.GetUploadImage.Object);
            //Action
            var result = await repo.GetEvent("5");
            //Assert
            Assert.True(ExtGetDataResult.EmptyResults == result.StatusResult);
        }

        [Fact]
        public async Task GetNotificationEventById_ReturnEmptyOnErrorDb()
        {
            //Arrange
            
            var repo = new NotificationEventOperations(MoqHelper.NotificationEvent.GetNotificationEventRepositoryThrowExceptionOnGetEvent().Object, 
                MoqHelper.Barrack.GetBarrackRepository.Object, 
                MoqHelper.PhenologicalEvent.GetPhenologicalEventRepository.Object, 
                MoqHelper.CommonDb.GetDbOperations<NotificationEvent>().Object, 
                MoqHelper.UpImage.GetUploadImage.Object);
            //Action
            var result = await repo.GetEvent("5");
            //Assert
            Assert.True(result.GetType() == typeof(ExtGetErrorContainer<NotificationEvent>));
        }

        [Fact]
        public async Task GetNotificationEvents_ReturnEventsSuccess()
        {
            //Arrange
            
            var repo = new NotificationEventOperations(MoqHelper.NotificationEvent.GetNotificationEventRepository.Object, 
                MoqHelper.Barrack.GetBarrackRepository.Object, 
                MoqHelper.PhenologicalEvent.GetPhenologicalEventRepository.Object, 
                MoqHelper.CommonDb.GetDbOperations<NotificationEvent>().Object, 
                MoqHelper.UpImage.GetUploadImage.Object);
            //Action
            var result = await repo.GetEvents();
            //Assert
            Assert.True(ExtGetDataResult.Success == result.StatusResult);
        }

        [Fact]
        public async Task GetNotificationEvents_ReturnEventsEmptyResults()
        {
            //Arrange
            
            var repo = new NotificationEventOperations(MoqHelper.NotificationEvent.GetNotificationEventRepositoryReturnEmptyOnGetEvents().Object, 
                MoqHelper.Barrack.GetBarrackRepository.Object, 
                MoqHelper.PhenologicalEvent.GetPhenologicalEventRepository.Object, 
                MoqHelper.CommonDb.GetDbOperations<NotificationEvent>().Object, 
                MoqHelper.UpImage.GetUploadImage.Object);
            //Action
            var result = await repo.GetEvents();
            //Assert
            Assert.True(ExtGetDataResult.EmptyResults == result.StatusResult);
        }

        [Fact]
        public async Task GetNotificationEvents_ReturnExceptionContainerOnNullList()
        {
            //Arrange
            
            var repo = new NotificationEventOperations(MoqHelper.NotificationEvent.GetNotificationEventRepositoryReturnNullOnGetEvents().Object, 
                MoqHelper.Barrack.GetBarrackRepository.Object, 
                MoqHelper.PhenologicalEvent.GetPhenologicalEventRepository.Object, 
                MoqHelper.CommonDb.GetDbOperations<NotificationEvent>().Object, 
                MoqHelper.UpImage.GetUploadImage.Object);
            //Action
            var result = await repo.GetEvents();

            //Assertt
            Assert.True(result.GetType() == typeof(ExtGetErrorContainer<List<NotificationEvent>>) && ((ExtGetErrorContainer<List<NotificationEvent>>)result).ErrorMessage.Contains("nulo"));
        }
        [Fact]
        public async Task GetNotificationEvents_ReturnExceptionContainerWhenDbException()
        {
            //Arrange            
            var repo = new NotificationEventOperations(MoqHelper.NotificationEvent.GetNotificationEventRepositoryThrowExceptionOnGetEvents().Object, 
                MoqHelper.Barrack.GetBarrackRepository.Object, 
                MoqHelper.PhenologicalEvent.GetPhenologicalEventRepository.Object, 
                MoqHelper.CommonDb.GetDbOperations<NotificationEvent>().Object, 
                MoqHelper.UpImage.GetUploadImage.Object);
            //Action
            var result = await repo.GetEvents();

            //Assertt
            Assert.True(result.GetType() == typeof(ExtGetErrorContainer<List<NotificationEvent>>));
        }

        [Theory]
        [InlineData(null,null,null,null)]
        [InlineData("", "", "", "")]
        public async Task SaveNewNotificationEvent_EmptyAndNullParameters_returnErrorContainer(string idBarrick, string idPhenologicalEvent, string base64, string description) {
            //Arrange

            var repo = new NotificationEventOperations(MoqHelper.NotificationEvent.GetNotificationEventRepository.Object, 
                MoqHelper.Barrack.GetBarrackRepository.Object, 
                MoqHelper.PhenologicalEvent.GetPhenologicalEventRepository.Object, 
                MoqHelper.CommonDb.GetDbOperations<NotificationEvent>().Object, 
                MoqHelper.UpImage.GetUploadImage.Object);
            //Action
            var result = await repo.SaveNewNotificationEvent(idBarrick, idPhenologicalEvent, base64, description);
            //Assert
            Assert.True(result.GetType() == typeof(ExtPostErrorContainer<string>));
        }


        [Theory]       
        [InlineData("a", "s", "d", "f")]
        public async Task SaveNewNotificationEvent_differentParameters_returnOk(string idBarrick, string idPhenologicalEvent, string base64, string description)
        {
            //Arrange            
            var repo = new NotificationEventOperations(
                MoqHelper.NotificationEvent.GetNotificationEventRepository.Object, 
                MoqHelper.Barrack.GetBarrackRepository.Object, 
                MoqHelper.PhenologicalEvent.GetPhenologicalEventRepository.Object, 
                MoqHelper.CommonDb.GetDbOperations<NotificationEvent>().Object, 
                MoqHelper.UpImage.GetUploadImage.Object);

            //Action
            var result = await repo.SaveNewNotificationEvent(idBarrick, idPhenologicalEvent, base64, description);
            //Assert
            Assert.True(result.MessageResult == ExtMessageResult.Ok);
        }










    }
}
