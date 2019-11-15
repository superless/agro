using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.events
{
    public interface INotificatonEventOperations
    {
        Task<ExtPostContainer<string>> SaveNewNotificationEvent(string idBarrack, string idPhenologicalEvent, string base64, string description);

        Task<ExtGetContainer<List<NotificationEvent>>> GetEvents();

        Task<ExtGetContainer<NotificationEvent>> GetEvent(string id);

    }
}
