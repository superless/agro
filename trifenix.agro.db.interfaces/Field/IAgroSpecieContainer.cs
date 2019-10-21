using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements;
using trifenix.agro.db.model.enforcements.Fields;

namespace trifenix.agro.db.interfaces.Field
{
    public interface IAgroSpecieContainer
    {
        Task<string> CreateUpdateSpecie(AgroSpecie specie);

        Task<AgroSpecie> Getspecie(string uniqueId);

        IQueryable<AgroSpecie> GetSpecies();

    }


}
