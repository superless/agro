using System.Collections.Generic;

namespace trifenix.agro.email.interfaces {
    public interface IEmail {
        void SendEmail(List<string> mails, string subject, string htmlBody);
    }
}