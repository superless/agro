using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.model.agro;
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqNotificationEvent
    {
        private readonly Mock<INotificationEventRepository> mockNotification;
        
        
        


        public MoqNotificationEvent()
        {
            
            mockNotification = new Mock<INotificationEventRepository>();
            
            mockNotification.Setup(s => s.GetNotificationEvent(It.IsAny<string>())).ReturnsAsync(FakeGenerator.GetNotificationEvent());
            mockNotification.Setup(s => s.GetNotificationEvents()).Returns(FakeGenerator.GetNotificationEvents());
            mockNotification.Setup(s => s.CreateUpdateNotificationEvent(It.IsAny<NotificationEvent>())).ReturnsAsync(FakeGenerator.CreateUpdateNotificationEvent());

           

            

            

        }

        public Mock<INotificationEventRepository> GetNotificationEventRepository => mockNotification;
        
        
        

        public Mock<INotificationEventRepository> GetNotificationEventRepositoryReturnNullOnGetEvent()
        {
            mockNotification.Setup(s => s.GetNotificationEvent(It.IsAny<string>())).ReturnsAsync((NotificationEvent)null);
            return mockNotification;
        }

        public Mock<INotificationEventRepository> GetNotificationEventRepositoryThrowExceptionOnGetEvent()
        {
            mockNotification.Setup(s => s.GetNotificationEvent(It.IsAny<string>())).Throws(new Exception("error on db"));
            return mockNotification;
        }

        public Mock<INotificationEventRepository> GetNotificationEventRepositoryReturnEmptyOnGetEvents()
        {
            mockNotification.Setup(s => s.GetNotificationEvents()).Returns(new List<NotificationEvent>().AsQueryable());
            return mockNotification;
        }

        public Mock<INotificationEventRepository> GetNotificationEventRepositoryThrowExceptionOnGetEvents()
        {
            mockNotification.Setup(s => s.GetNotificationEvents()).Throws(new Exception("error on db"));
            return mockNotification;
        }

        public Mock<INotificationEventRepository> GetNotificationEventRepositoryReturnNullOnGetEvents()
        {
            mockNotification.Setup(s => s.GetNotificationEvents()).Returns((IQueryable<NotificationEvent>)null);
            return mockNotification;
        }
    }
}
