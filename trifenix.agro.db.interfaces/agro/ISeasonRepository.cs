using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface ISeasonRepository
    {

        Task<string> CreateUpdateSeason(Season season);

        Task<Season> GetSeason(string id);

        IQueryable<Season> GetSeasons();

        Task<Season> GetCurrentSeason();

    }
}
