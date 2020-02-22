using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model;

namespace trifenix.agro.db.interfaces.agro
{
    public interface ISpecieRepository
    {
        Task<string> CreateUpdateSpecie(Specie specie);

        Task<Specie> GetSpecie(string id);

        IQueryable<Specie> GetSpecies();
    }


}
