using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface ISpecieOperations
    {
        Task<ExtPostContainer<string>> SaveNewSpecie(string name);

        Task<ExtPostContainer<Specie>> SaveEditSpecie(string id, string name);

        Task<ExtGetContainer<List<Specie>>> GetSpecies();
        
    }
}
