using System.Threading.Tasks;
using trifenix.agro.db.model;

namespace trifenix.agro.db.interfaces.agro {
    public interface ICounterRepository {

        Task<string> CreateUpdateCounter(Counter Counter);
        Counter GetCounter();

    }
}