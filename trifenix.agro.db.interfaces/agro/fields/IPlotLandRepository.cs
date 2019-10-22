using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro.fields
{
    public interface IPlotLandRepository
    {

        Task<string> CreateUpdateSector(PlotLand plotLand);

        Task<PlotLand> GetPlotLand(string id);

        IQueryable<PlotLand> GetPlotLands();

    }
}
