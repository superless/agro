using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using trifenix.agro.email.interfaces;

namespace trifenix.agro.email.operations {
    public class Email : IEmail {

        private MailMessage Mail;
        private SmtpClient SmtpServer;

        public Email(string sender, string password) {
            InitSMTPServer(sender,password);
        }

        private void InitSMTPServer(string sender, string password) {
            Mail = new MailMessage();
            Mail.From = new MailAddress("aresa.notificaciones@gmail.com", "Aresa");
            Mail.IsBodyHtml = true;
            SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.Credentials = new System.Net.NetworkCredential(sender,password);
            SmtpServer.EnableSsl = true;
        }

        public void SendEmail(List<string> mails, string subject, string htmlBody) {
            Mail.Subject = subject;
            var receivers = mails.Select(mail => new MailAddress(mail)).ToList();
            receivers.ForEach(receiver => Mail.To.Add(receiver));
            Mail.Body = htmlBody;
            SmtpServer.Send(Mail);
        }

    }
}