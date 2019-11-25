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
            using (requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/jhmad.onmicrosoft.com/oauth2/v2.0/token"))
            {
                var parameters = new Dictionary<string, string> {
                    { "client_id", "a81f0ad4-912b-46d3-ba3e-7bf605693242" },
                    { "scope", "https://graph.microsoft.com/.default" },
                    { "client_secret", "gUjIa4F=NXlAwwMCF2j2SFMMj3?m@=FM" },
                    { "grant_type", "client_credentials" },
                };
                requestMessage.Content = new FormUrlEncodedContent(parameters);
                dynamic response = JsonConvert.DeserializeObject(await (await client.SendAsync(requestMessage)).Content.ReadAsStringAsync());
                bearerToken = response.access_token;
            }
            using (requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/directoryRoles/1add1d5e-8da1-4495-b33e-4bd26ad894eb/members"))
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