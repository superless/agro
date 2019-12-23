using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface INebulizerRepository
    {
        Task<string> CreateUpdateNebulizer(Nebulizer Nebulizer);

        Task<Nebulizer> GetNebulizer(string id);

        IQueryable<Nebulizer> GetNebulizers();
    }
}
