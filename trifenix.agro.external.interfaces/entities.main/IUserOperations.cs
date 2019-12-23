using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface IUserOperations
    {
        Task<ExtPostContainer<string>> SaveNewUser(string name, string rut, string email, string idJob, string[] idsRoles, string idNebulizer, string idTractor);

        Task<ExtPostContainer<User>> SaveEditUser(string id, string name, string rut, string email, string idJob, string[] idsRoles, string idNebulizer, string idTractor);

        Task<ExtGetContainer<User>> GetUser(string id);

        Task<ExtGetContainer<List<User>>> GetUsers();

    }
}
