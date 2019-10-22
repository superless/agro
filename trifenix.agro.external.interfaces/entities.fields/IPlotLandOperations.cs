using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.fields
{
    public interface IPlotLandOperations
    {
        Task<ExtPostContainer<string>> SaveNewPlotLand(string name, string idSector);

        Task<ExtPostContainer<PlotLand>> SaveEditPlotLand(string id, string name, string idSector);

        Task<ExtGetContainer<List<PlotLand>>> GetPlotLands();

        Task<ExtGetContainer<PlotLand>> GetPlotLand(string id);
    }
}
