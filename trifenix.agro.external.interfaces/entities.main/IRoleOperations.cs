using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main {
    public interface IRoleOperations {
        Task<ExtPostContainer<string>> SaveNewRole(string name);

        Task<ExtPostContainer<Role>> SaveEditRole(string id, string name);

        Task<ExtGetContainer<Role>> GetRole(string id);

        Task<ExtGetContainer<List<Role>>> GetRoles();

    }
}