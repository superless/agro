using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro {
    public interface IRoleRepository {
        Task<string> CreateUpdateRole(Role Role);
        Task<Role> GetRole(string id);
        IQueryable<Role> GetRoles();

    }
}