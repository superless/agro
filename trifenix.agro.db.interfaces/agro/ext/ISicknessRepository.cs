using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro.ext
{
    public interface ISicknessRepository
    {
        Task<string> CreateUpdateSickness(Sickness product);

        Task<Sickness> GetSickness(string id);

        IQueryable<Sickness> GetSicknesses();
    }
}
