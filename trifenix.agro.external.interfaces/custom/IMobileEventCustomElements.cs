using System.Threading.Tasks;
using trifenix.agro.model.external;
using trifenix.agro.model.external.output;

namespace trifenix.agro.external.interfaces.custom
{
    public interface IMobileEventCustomElements
    {
        Task<ExtGetContainer<long>> GetMobileEventTimestamp();

        Task<ExtGetContainer<EventInitData>> GetEventData();


        Task<ExtGetContainer<NotificationCustomPhenologicalResult>> GetNotificationPreOrdersResult(string idSpecie, int page, int elementsByPage, bool orderDateDesc);
    }
}
