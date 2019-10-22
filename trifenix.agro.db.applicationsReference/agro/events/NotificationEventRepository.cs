using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.events
{
    public class NotificationEventRepository : MainDb<NotificationEvent>, INotificationEventRepository
    {
        public NotificationEventRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateNotificationEvent(NotificationEvent notificationEvent)
        {
            return await CreateUpdate(notificationEvent);
        }

        public async Task<NotificationEvent> GetNotificationEvent(string id)
        {
            return await GetEntity(id);
        }

        public IQueryable<NotificationEvent> GetNotificationEvents()
        {
            return GetEntities();
        }
    }
}
