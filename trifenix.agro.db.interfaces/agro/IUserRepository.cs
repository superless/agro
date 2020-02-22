using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IUserRepository
    {
        Task<string> CreateUpdateUser(UserApplicator user);

        Task<UserApplicator> GetUser(string id);

        Task<UserApplicator> GetUserFromToken(string objectId);

        IQueryable<UserApplicator> GetUsers();

    }
}
