using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface ISeasonOperations
    {
        Task<ExtPostContainer<string>> SaveNewSeason(DateTime init, DateTime end);

        Task<ExtPostContainer<Season>> SaveEditSeason(string id, DateTime init, DateTime end, bool current);

        Task<ExtGetContainer<List<Season>>> GetSeasons();
    }
}