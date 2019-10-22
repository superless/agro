using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.interfaces.agro.fields;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.applicationsReference.agro.fields
{
    public class PlotLandRepository : MainDb<PlotLand>, IPlotLandRepository
    {
        public PlotLandRepository(AgroDbArguments args) : base(args)
        {
        }

        public async Task<string> CreateUpdateSector(PlotLand plotLand)
        {
            return await CreateUpdate(plotLand);
        }

        public async Task<PlotLand> GetPlotLand(string id)
        {
            return await GetEntity(id);
        }

        public IQueryable<PlotLand> GetPlotLands()
        {
            return GetEntities();
        }
    }
}
