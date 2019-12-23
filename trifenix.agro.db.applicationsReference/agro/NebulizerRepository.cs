using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class NebulizerRepository : INebulizerRepository
    {

        private readonly IMainDb<Nebulizer> _db;
        public NebulizerRepository(IMainDb<Nebulizer> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateNebulizer(Nebulizer Nebulizer)
        {
            return await _db.CreateUpdate(Nebulizer);
        }

        public async Task<Nebulizer> GetNebulizer(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<Nebulizer> GetNebulizers()
        {
            return _db.GetEntities();
        }
    }
}
