using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IRootstockRepository
    {
        Task<string> CreateUpdateRootstock(Rootstock rootstock);

        Task<Rootstock> GetRootstock(string id);

        IQueryable<Rootstock> GetRootstocks();
    }


}
