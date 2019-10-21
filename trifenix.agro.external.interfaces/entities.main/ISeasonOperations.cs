using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface ISeasonOperations
    {
        Task<ExtPostContainer<string>> SaveNewSeason(string name);

        Task<ExtPostContainer<Season>> SaveEditSeason(string id, string name);


        Task<ExtGetContainer<List<Season>>> GetSeasons();
    }
}