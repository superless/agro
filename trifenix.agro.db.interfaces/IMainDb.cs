using Cosmonaut;
using System.Linq;
using System.Threading.Tasks;

namespace trifenix.agro.db.interfaces {

    public interface IMainGenericDb<T> where T : DocumentBase {

        ICosmosStore<T> Store { get; }
        Task<string> CreateUpdate(T entity);
        Task<T> GetEntity(string uniqueId);
        IQueryable<T> GetEntities();

    }

}