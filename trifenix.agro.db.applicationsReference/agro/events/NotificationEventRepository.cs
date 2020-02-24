using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.events;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.events
{


    public class NotificationEventRepository : INotificationEventRepository
    {

        private readonly IMainDb<NotificationEvent> _db;

        public NotificationEventRepository(AgroDbArguments dbArguments)
        {
            _db = new MainDb<NotificationEvent>(dbArguments);
        }

        public async Task<string> CreateUpdateNotificationEvent(NotificationEvent notificationEvent)
        {
            return await _db.CreateUpdate(notificationEvent);
        }

        public async Task<NotificationEvent> GetNotificationEvent(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<NotificationEvent> GetNotificationEvents()
        {
            return _db.GetEntities();
        }

       

        public async Task<long> Total(string season, string idSpecie)
        {
            return await _db.Store.QuerySingleAsync<long>($"SELECT value count(1) FROM c where c.Barrack.SeasonId = '{season}' and c.Barrack.Variety.Specie.Id = '{idSpecie}'");
        }
    }
}
