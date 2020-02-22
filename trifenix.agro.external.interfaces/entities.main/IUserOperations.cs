using System.Collections.Generic;
using System.Threading.Tasks;
using trifenix.agro.db.model;
using trifenix.agro.model.external;

namespace trifenix.agro.external.interfaces.entities.main
{
    public interface IUserOperations
    {
        Task<ExtPostContainer<string>> SaveNewUser(string name, string rut, string email, string idJob, string[] idsRoles, string idNebulizer, string idTractor);

        Task<ExtPostContainer<UserApplicator>> SaveEditUser(string id, string name, string rut, string email, string idJob, string[] idsRoles, string idNebulizer, string idTractor);

        Task<ExtGetContainer<UserApplicator>> GetUser(string id);

        Task<ExtGetContainer<UserApplicator>> GetUserFromToken();

        Task<ExtGetContainer<List<UserApplicator>>> GetUsers();

    }
}