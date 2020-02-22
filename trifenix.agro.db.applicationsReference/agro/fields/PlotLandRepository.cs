using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model;

namespace trifenix.agro.db.applicationsReference.agro.fields
{
    public class PlotLandRepository : IPlotLandRepository
    {
        private readonly IMainDb<PlotLand> _db;
        public PlotLandRepository(IMainDb<PlotLand> db) 
        {
            _db = db;
        }

        public async Task<string> CreateUpdateSector(PlotLand plotLand)
        {
            return await _db.CreateUpdate(plotLand);
        }

        public async Task<PlotLand> GetPlotLand(string id)
        {
            return await _db.GetEntity(id);
        }

        public IQueryable<PlotLand> GetPlotLands()
        {
            return _db.GetEntities();
        }
    }
}
