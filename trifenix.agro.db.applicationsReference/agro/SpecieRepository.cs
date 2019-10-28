using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class SpecieRepository : ISpecieRepository
    {

        private readonly IMainDb<Specie> _db;
        public SpecieRepository(IMainDb<Specie> db)         
        {
            _db = db;
        }

        public async Task<string> CreateUpdateSpecie(Specie specie)
        {
            return await _db.CreateUpdate(specie);
        }

        public async Task<Specie> GetSpecie(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<Specie> GetSpecies()
        {
            return _db.GetEntities();
        }
    }
}
