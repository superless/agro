using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.enforcements.@base;

namespace trifenix.agro.db.interfaces.@base
{
    public interface IAgroSeasonContainer
    {
        Task<string> CreateUpdateSeason(AgroYear season);

        Task<AgroYear> GetSeason(string uniqueId);

        IQueryable<AgroYear> GetSeasons();

    }
}
