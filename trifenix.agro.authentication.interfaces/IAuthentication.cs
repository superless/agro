using System.Security.Claims;
using System.Threading.Tasks;

namespace trifenix.agro.authentication.interfaces
{
    public interface IAuthentication
    {
        Task<ClaimsPrincipal> ValidateAccessToken(string accessToken);

    }
}
