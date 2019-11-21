using Cosmonaut;
using System.Linq;
using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces
{
    public interface IMainDb<T> where T:DocumentBase
    {
        Task<string> CreateUpdate(T entity);
        Task<T> GetEntity(string uniqueId);
        IQueryable<T> GetEntities();

        Task<long> GetTotalElements();

        ICosmosStore<T> Store { get; }
    }
}
