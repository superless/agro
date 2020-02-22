using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model;

namespace trifenix.agro.db.interfaces.agro.fields
{
    public interface IBarrackRepository {

        Task<string> CreateUpdateBarrack(Barrack barrack);

        Task<Barrack> GetBarrack(string id);

        IQueryable<Barrack> GetBarracks();
    }
}
