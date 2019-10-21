using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface ISpecieRepository
    {
        Task<string> CreateUpdateSpecie(Specie specie);

        Task<Specie> GetSpecie(string id);

        IQueryable<Specie> GetSpecies();
    }


}
