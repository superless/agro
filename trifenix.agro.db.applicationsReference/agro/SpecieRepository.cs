using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class SpecieRepository : MainDb<Specie>, ISpecieRepository
    {
        public SpecieRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateSpecie(Specie specie)
        {
            return await CreateUpdate(specie);
        }

        public async Task<Specie> GetSpecie(string id)
        {
            return await GetEntity(id);
        }

        public IQueryable<Specie> GetSpecies()
        {
            return GetEntities();
        }
    }
}
