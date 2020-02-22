using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using trifenix.agro.db.model;
using trifenix.agro.email.interfaces;

namespace trifenix.agro.email.operations {
    public class Email : IEmail {
        private readonly MailMessage Mail = new MailMessage();
        private readonly SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        private readonly List<UserApplicator> Users;

        public Email(List<UserApplicator> users) {
            Users = users;
            InitSMTPServer();
        }

        private void InitSMTPServer() {
            Mail.From = new MailAddress("aresa.notificaciones@gmail.com", "Aresa");
            Mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("aresa.notificaciones@gmail.com", "Aresa2019");
            SmtpServer.EnableSsl = true;
        }
        public IActionResult SendEmail(string subject, string htmlBody) {
            Mail.Subject = subject;
            var receivers = GetReceivers(new List<string> { "Rol para notificaciones" });
            receivers.ForEach(delegate (MailAddress user) {
                Mail.To.Add(user);
            });
            Mail.Body = htmlBody;
            try {
                SmtpServer.Send(Mail);
                return new OkObjectResult("Ok!");
            }
            catch (Exception ex) {
                Console.WriteLine("Ha ocurrido la siguiente excepcion:\n" + ex.StackTrace);
                return new ConflictObjectResult(ex);
            }
        }

        private List<MailAddress> GetReceivers(List<string> roles) {
            List<MailAddress> mails = new List<MailAddress>();
            Users.FindAll(user => user.Roles.Any(role => roles.Contains(role.Name))).ForEach(userReceiver => mails.Add(new MailAddress(userReceiver.Email)));
            return mails;
        }
    }

}