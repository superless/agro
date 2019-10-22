using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface IVarietyOperations
    {
        Task<ExtPostContainer<string>> SaveNewVariety(string name, string abbreviation, string idSpecie);

        Task<ExtPostContainer<Variety>> SaveEditVariety(string id, string name, string abbreviation, string idSpecie);

        Task<ExtGetContainer<List<Variety>>> GetVarieties();

        Task<ExtGetContainer<Variety>> GetVariety(string id);
    }
}
