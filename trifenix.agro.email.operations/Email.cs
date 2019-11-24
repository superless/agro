using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using trifenix.agro.email.interfaces;

namespace trifenix.agro.email.operations
{
    public class Email : IEmail
    {
        private readonly MailMessage mail = new MailMessage();
        private readonly SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        private readonly HttpClient client = new HttpClient();

        public Email()
        {
            initSMTPServer();
        }

        private void initSMTPServer() {
            mail.From = new MailAddress("aresa.notificaciones@gmail.com", "Aresa");
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("aresa.notificaciones@gmail.com", "Aresa2019");
            SmtpServer.EnableSsl = true;
        }
        public async Task<IActionResult> SendEmail(string subject, string htmlBody)
        {
            mail.Subject = subject;
            var receivers = await getReceivers();
            receivers.ForEach(delegate (MailAddress user) {
                mail.To.Add(user);
            });
            mail.Body = htmlBody;
            try{
                SmtpServer.Send(mail);
                return new OkObjectResult("Ok!");
            }
            catch (Exception ex){
                Console.WriteLine("Ha ocurrido la siguiente excepcion:\n" + ex.StackTrace);
                return new ConflictObjectResult(ex);
            }
        }

        private async Task<List<MailAddress>> getReceivers()
        {
            string bearerToken = "";
            HttpRequestMessage requestMessage;
            using (requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/aresatest.onmicrosoft.com/oauth2/v2.0/token"))
            {
                var parameters = new Dictionary<string, string> {
                    { "client_id", "8247a3f6-7aa4-4399-8849-894e702ec793" },
                    { "scope", "https://graph.microsoft.com/.default" },
                    { "client_secret", "i39j]Rq9B1M=IPNQxsCwvXX@_rKc1ZwZ" },
                    { "grant_type", "client_credentials" },
                };
                requestMessage.Content = new FormUrlEncodedContent(parameters);
                dynamic response = JsonConvert.DeserializeObject(await (await client.SendAsync(requestMessage)).Content.ReadAsStringAsync());
                bearerToken = response.access_token;
            }
            using (requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/directoryRoles/df87464f-16e5-467e-b7f5-3c704ba3d099/members"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                dynamic response = JsonConvert.DeserializeObject(await (await client.SendAsync(requestMessage)).Content.ReadAsStringAsync());
                List<MailAddress> mails = new List<MailAddress>();
                for (int index = 0; index < response.value.Count; index++)
                    if (response.value[index].mail != null)
                        mails.Add(new MailAddress(response.value[index].mail.ToString()));
                return mails;
            }
        }
    }
}