using Cosmonaut;
using System.Linq;
using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces {

    public interface IMainGenericDb<T> where T : DocumentBase {

        Task<string> CreateUpdate(T entity);
        Task<T> GetEntity(string uniqueId);
        ICosmosStore<T> Store { get; }
        IQueryable<T> GetEntities();

        Task DeleteEntity(string id);

    }

}