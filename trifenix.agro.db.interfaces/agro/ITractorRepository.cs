using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model;

namespace trifenix.agro.db.interfaces.agro
{
    public interface ITractorRepository
    {
        Task<string> CreateUpdateTractor(Tractor Tractor);

        Task<Tractor> GetTractor(string id);

        IQueryable<Tractor> GetTractors();
    }
}
