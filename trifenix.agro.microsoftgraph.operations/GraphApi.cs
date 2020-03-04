using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.agro.db.applicationsReference;
using trifenix.agro.db.applicationsReference.agro.Common;
using trifenix.agro.db.interfaces;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.db.model.agro;
using trifenix.agro.microsoftgraph.interfaces;

namespace trifenix.agro.microsoftgraph.operations
{
    public class GraphApi : IGraphApi {

        private readonly IConfidentialClientApplication _confidentialClientApplication;
        private readonly ICommonQueries queries;
        private readonly IMainGenericDb<UserApplicator> repo;

        public ClaimsPrincipal AccessTokenClaims { get; set; }

        public GraphApi(ClaimsPrincipal accessTokenClaim, AgroDbArguments arguments) {
            AccessTokenClaims = accessTokenClaim;
            this.queries = new CommonQueries(arguments);
            this.repo = new MainGenericDb<UserApplicator>(arguments);
            _confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process))
                .WithAuthority("https://login.microsoftonline.com/" + Environment.GetEnvironmentVariable("tenantID", EnvironmentVariableTarget.Process) + "/v2.0")
                .WithClientSecret(Environment.GetEnvironmentVariable("clientSecret", EnvironmentVariableTarget.Process))
                .Build();
        }

        public async Task<UserApplicator> GetUserFromToken() {
            //TODO corregir
            //try {
            //    string objectIdAAD = AccessTokenClaims.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //    var idUSer = await queries.GetUserIdFromAAD(objectIdAAD);
            //    var tmpUser = await repo.GetEntity(idUSer);
            //    return tmpUser;
            //}
            //catch (Exception ex) {
            //    Console.WriteLine("Error en GetUserFromToken():\n" + ex.StackTrace);
            //}
            return null;
        }

        public async Task<string> CreateUserIntoActiveDirectory(string name, string email) {
            var scopes = new string[] { "https://graph.microsoft.com/.default" };
            var authResult = await _confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
            var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) => 
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken)));


            var invitation = new Invitation {
                InvitedUserDisplayName = name,
                InvitedUserEmailAddress = email,
                InvitedUserMessageInfo = new InvitedUserMessageInfo() { CustomizedMessageBody = "Bienvenido a Aresa", MessageLanguage = "es-es"},
                InviteRedirectUrl = "https://sprint3-jhm.trifenix.io/",
                SendInvitationMessage = true,
            };
            await graphServiceClient.Invitations.Request().AddAsync(invitation);
            string objectId = String.Empty;
            do {
                Thread.Sleep(1000);
                objectId = await GetObjectIdFromEmail(email);
            } while (String.IsNullOrEmpty(objectId));
            return objectId;
        }

        private async Task<string> GetObjectIdFromEmail(string email) {
            var scopes = new string[] { "https://graph.microsoft.com/.default" };
            var authResult = await _confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
            HttpClient client = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users/");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            var response = await client.SendAsync(requestMessage);
            client.Dispose();
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(responseBody);
            JArray jArray = json.value?.ToObject<JArray>();
            var jUser = jArray.FirstOrDefault(user => !String.IsNullOrEmpty(user.Value<string>("mail"))?user.Value<string>("mail").Equals(email):false);
            return jUser?.Value<string>("id");
        }
    }
}