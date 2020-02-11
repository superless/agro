using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.db.model.agro.enums;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.core
{
    public interface IBusinessNameOperations
    {

        // borrar y cambiar
        Task<ExtPostContainer<string>> SaveNewNotificationEvent(string idPhenologicalEvent, NotificationType NotificationType, string idBarrack, string base64, string description, float lat, float lon);

        Task<ExtGetContainer<List<NotificationEvent>>> GetEvents();

        Task<ExtGetContainer<NotificationEvent>> GetEvent(string id);

        Task<ExtGetContainer<List<NotificationEvent>>> GetEventsByBarrackId(string id);

        Task<ExtGetContainer<List<NotificationEvent>>> GetEventsByBarrackPhenologicalEventId(string idBarrack, string idPhenologicalId);

    }
}
