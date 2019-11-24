using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace trifenix.agro.email.interfaces
{
    public interface IEmail
    {
        Task<IActionResult> SendEmail(string subject, string htmlBody);
    }
}
