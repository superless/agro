using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.external.operations.tests.helper.staticHelper
{
    public static class MoqNotificationEvent
    {   
        public static Mock<INotificationEventRepository> GetNotificationEventReturnNullResults()
        {
            var mockNotification = new Mock<INotificationEventRepository>();
            mockNotification.Setup(s => s.GetNotificationEvent(It.IsAny<string>())).ReturnsAsync((NotificationEvent)null);
            mockNotification.Setup(s => s.GetNotificationEvents()).Returns((IQueryable<NotificationEvent>)null);
            mockNotification.Setup(s => s.CreateUpdateNotificationEvent(It.IsAny<NotificationEvent>())).ReturnsAsync((string)null);
            return mockNotification;
        }

        public static Mock<INotificationEventRepository> GetNotificationEventReturnResults()
        {
            var mockNotification = new Mock<INotificationEventRepository>();
            mockNotification.Setup(s => s.GetNotificationEvent(It.IsAny<string>())).ReturnsAsync(FakeGenerator.GetElement<NotificationEvent>());
            mockNotification.Setup(s => s.GetNotificationEvents()).Returns(FakeGenerator.GetElements<NotificationEvent>());
            mockNotification.Setup(s => s.CreateUpdateNotificationEvent(It.IsAny<NotificationEvent>())).ReturnsAsync(FakeGenerator.CreateString());
            return mockNotification;
        }

        public static Mock<INotificationEventRepository> GetNotificationEventReturnEmpty()
        {
            var mockNotification = new Mock<INotificationEventRepository>();
            mockNotification.Setup(s => s.GetNotificationEvent(It.IsAny<string>())).ReturnsAsync((NotificationEvent)null);
            mockNotification.Setup(s => s.GetNotificationEvents()).Returns(new List<NotificationEvent>().AsQueryable());
            mockNotification.Setup(s => s.CreateUpdateNotificationEvent(It.IsAny<NotificationEvent>())).ReturnsAsync(string.Empty);
            return mockNotification;
        }

        public static Mock<INotificationEventRepository> GetNotificationEventThrowException()
        {
            var mockNotification = new Mock<INotificationEventRepository>();
            mockNotification.Setup(s => s.GetNotificationEvent(It.IsAny<string>())).Throws(new Exception("simulated test error"));
            mockNotification.Setup(s => s.GetNotificationEvents()).Throws(new Exception("simulated test error"));
            mockNotification.Setup(s => s.CreateUpdateNotificationEvent(It.IsAny<NotificationEvent>())).Throws(new Exception("simulated test error"));
            return mockNotification;
        }
    }
}
