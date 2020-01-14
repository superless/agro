using Microsoft.AspNetCore.Mvc;

namespace trifenix.agro.email.interfaces {
    public interface IEmail {
        IActionResult SendEmail(string subject, string htmlBody);

    }

}