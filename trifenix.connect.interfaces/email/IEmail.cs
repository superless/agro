using System.Collections.Generic;

namespace trifenix.connect.interfaces
{
    public interface IEmail {
        void SendEmail(List<string> mails, string subject, string htmlBody);
    }
}