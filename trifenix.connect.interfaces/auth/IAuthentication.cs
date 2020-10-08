using System.Security.Claims;
using System.Threading.Tasks;

namespace trifenix.connect.interfaces.auth
{
    public interface IAuthentication
    {
        Task<ClaimsPrincipal> ValidateAccessToken(string accessToken);

    }
}
