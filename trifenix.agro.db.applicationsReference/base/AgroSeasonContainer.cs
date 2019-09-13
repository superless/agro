using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.@base;
using trifenix.agro.db.model.enforcements.@base;

namespace trifenix.agro.db.applicationsReference.@base
{
    public class AgroSeasonContainer : MainDb<AgroYear>, IAgroSeasonContainer
    {
        public AgroSeasonContainer(AgroDbArguments dbArguments) : base(dbArguments)
        {

        }

        public async Task<string> CreateUpdateSeason(AgroYear season)
        {
            return await CreateUpdate(season);
        }

        public async Task<AgroYear> GetSeason(string uniqueId)
        {
            return await GetEntity(uniqueId);
        }

        public IQueryable<AgroYear> GetSeasons()
        {
            return GetEntities();
        }
    }
}
