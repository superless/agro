using Cosmonaut;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;

namespace trifenix.connect.interfaces.db.cosmos
{

    public interface IMainGenericDb<T> where T : DocumentBase {

        ICosmosStore<T> Store { get; }


        Task<string> CreateUpdate(T entity);

        

        Task<string> CreateEntityContainer(T entity);


        Task<T> GetEntity(string uniqueId);
        IQueryable<T> GetEntities();

        

        Task DeleteEntity(string id);

    }

}