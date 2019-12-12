using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using trifenix.agro.microsoftgraph.interfaces;
using trifenix.agro.microsoftgraph.model;

namespace trifenix.agro.microsoftgraph.operations
{
    public class GraphApi : IGraphApi
    {
        private ClaimsPrincipal _accessTokenClaims;
        public GraphApi(ClaimsPrincipal accessTokenClaim)
        {
            AccessTokenClaims = accessTokenClaim;
        }
        public ClaimsPrincipal AccessTokenClaims
        {
            get => _accessTokenClaims;
            set { _accessTokenClaims = value; }
        }
        private async Task<string> GetRoleName(string idRole)
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>{
                { "grant_type", "client_credentials" },
                { "scope", "https://graph.microsoft.com/.default" },
                { "client_id", "a81f0ad4-912b-46d3-ba3e-7bf605693242" },
                { "client_secret", "gUjIa4F=NXlAwwMCF2j2SFMMj3?m@=FM" }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://login.microsoftonline.com/jhmad.onmicrosoft.com/oauth2/v2.0/token", content);
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(responseBody);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/directoryRoles");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", (string)json.access_token);
            response = await client.SendAsync(requestMessage);
            client.Dispose();
            responseBody = await response.Content.ReadAsStringAsync();
            json = JsonConvert.DeserializeObject(responseBody);
            var arrayOfRoles = (JArray)json.value;
            string roleName = ((JObject)arrayOfRoles.FirstOrDefault(r => r["roleTemplateId"].ToString().Equals(idRole))).Value<string>("displayName");
            string roleTransformation = Environment.GetEnvironmentVariable("roleTransformation", EnvironmentVariableTarget.Process);
            json = JsonConvert.DeserializeObject(roleTransformation);
            return json[roleName];
        }
        public async Task<User> GetUserInfo()
        {
            try
            {
                string name = AccessTokenClaims.FindFirst("name").Value;
                string email = AccessTokenClaims.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                List<string> roleNames = new List<string>();
                foreach (Claim claim in AccessTokenClaims.FindAll("wids"))
                    roleNames.Add(await GetRoleName(claim.Value));
                return new User(name, email, roleNames);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en GetUserInfo():\n" + ex.StackTrace);
            }
            return null;
        }
    }
}