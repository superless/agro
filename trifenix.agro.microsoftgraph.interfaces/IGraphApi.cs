using System.Threading.Tasks;

namespace trifenix.agro.microsoftgraph.interfaces {
    public interface IGraphApi {

        string GetUserId();
        Task<string> CreateUserIntoActiveDirectory(string name, string email);

    }
}