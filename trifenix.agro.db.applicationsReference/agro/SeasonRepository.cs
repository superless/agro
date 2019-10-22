using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model.agro;
using Cosmonaut.Exceptions;
using Cosmonaut.Extensions;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class SeasonRepository : MainDb<Season>, ISeasonRepository
    {

        public SeasonRepository(AgroDbArguments args) : base(args)
        {

        }
        public async Task<string> CreateUpdateSeason(Season season)
        {
            return await CreateUpdate(season);
        }

        public async Task<Season> GetCurrentSeason()
        {
            return await GetEntities().FirstOrDefaultAsync(s => s.Current);
        }

        public async Task<Season> GetSeason(string id)
        {
            return await GetEntity(id);
        }

        public IQueryable<Season> GetSeasons()
        {
            return GetEntities();
        }
    }
}
