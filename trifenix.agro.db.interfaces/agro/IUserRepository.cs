using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.model.agro;

namespace trifenix.agro.db.interfaces.agro
{
    public interface IUserRepository
    {
        Task<string> CreateUpdateUser(User user);

        Task<User> GetUser(string id);

        IQueryable<User> GetUsers();

    }
}
