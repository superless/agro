using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.fields
{
    public class BarrackRepository :  IBarrackRepository
    {

        private readonly IMainDb<Barrack> _db;
        public BarrackRepository(IMainDb<Barrack> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateBarrack(Barrack barrack)
        {
            return await _db.CreateUpdate(barrack);
        }

        public async Task<Barrack> GetBarrack(string id)
        {
           return  await _db.GetEntity(id);
        }

        public IQueryable<Barrack> GetBarracks()
        {
            return _db.GetEntities();
        }
    }
}
