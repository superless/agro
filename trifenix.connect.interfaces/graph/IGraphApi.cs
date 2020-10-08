using System.Threading.Tasks;

namespace trifenix.connect.interfaces.graph
{
    public interface IGraphApi {

        Task<string> CreateUserIntoActiveDirectory(string name, string email);

    }
}