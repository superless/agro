using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.events
{
    public interface INotificatonEventOperations
    {
        Task<ExtPostContainer<string>> SaveNewNotificationEvent(string idPhenologicalEvent, int eventType, string idBarrack, string base64, string description, float lat, float lon);

        Task<ExtGetContainer<List<NotificationEvent>>> GetEvents();

        Task<ExtGetContainer<NotificationEvent>> GetEvent(string id);

        Task<ExtGetContainer<List<NotificationEvent>>> GetEventsByBarrackId(string id);

        Task<ExtGetContainer<List<NotificationEvent>>> GetEventsByBarrackPhenologicalEventId(string idBarrack, string idPhenologicalId);


    }
}
