using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro.events
{
    public interface INotificationEventRepository
    {

        Task<string> CreateUpdateNotificationEvent(NotificationEvent notificationEvent);

        Task<NotificationEvent> GetNotificationEvent(string id);

        IQueryable<NotificationEvent> GetNotificationEvents();

        

        Task<long> Total(string season, string idSpecie);
    }
}
