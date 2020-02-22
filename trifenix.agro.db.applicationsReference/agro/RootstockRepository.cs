using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class RootstockRepository : IRootstockRepository
    {

        private readonly IMainDb<Rootstock> _db;
        public RootstockRepository(IMainDb<Rootstock> db)         
        {
            _db = db;
        }

        public async Task<string> CreateUpdateRootstock(Rootstock rootstock)
        {
            return await _db.CreateUpdate(rootstock);
        }

        public async Task<Rootstock> GetRootstock(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<Rootstock> GetRootstocks()
        {
            return _db.GetEntities();
        }
    }
}
