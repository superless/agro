using System.Threading.Tasks;
using trifenix.agro.microsoftgraph.model;

namespace trifenix.agro.microsoftgraph.interfaces
{
    public interface IGraphApi
    {
        Task<User> GetUserInfo();

        Task<bool> CreateUserIntoActiveDirectory();
    }
}