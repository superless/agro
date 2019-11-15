using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface ISpecieOperations
    {
        Task<ExtPostContainer<string>> SaveNewSpecie(string name, string abbreviation);

        Task<ExtPostContainer<Specie>> SaveEditSpecie(string id, string name, string abbreviation);

        Task<ExtGetContainer<List<Specie>>> GetSpecies();
        
    }

}
