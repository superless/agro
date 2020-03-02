using System.Threading.Tasks;

using trifenix.agro.db.model.agro;

namespace trifenix.agro.microsoftgraph.interfaces {
    public interface IGraphApi {
        Task<UserApplicator> GetUserFromToken();

        Task<string> CreateUserIntoActiveDirectory(string name, string email);

    }
}