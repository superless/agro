using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro;
using trifenix.agro.db.model;
using Cosmonaut.Exceptions;
using Cosmonaut.Extensions;
using trifenix.agro.db.interfaces;

namespace trifenix.agro.db.applicationsReference.agro
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly IMainDb<Season> _db;
        public SeasonRepository(IMainDb<Season> db)
        {
            _db = db;
        }
        public async Task<string> CreateUpdateSeason(Season season)
        {
            return await _db.CreateUpdate(season);
        }

        public async Task<Season> GetCurrentSeason()
        {
            return await _db.GetEntities().FirstOrDefaultAsync(s => s.Current);
        }

        public async Task<Season> GetSeason(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<Season> GetSeasons()
        {
            return _db.GetEntities();
        }
    }
}
