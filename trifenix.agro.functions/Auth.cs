using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using trifenix.agro.authentication.interfaces;
using trifenix.agro.authentication.operations;

namespace trifenix.agro.functions
{
    public static class Auth
    {
        private static bool MustBeAuthenticated() => bool.Parse(Environment.GetEnvironmentVariable("mustBeAuthenticated", EnvironmentVariableTarget.Process));
        
        //Recibe como parametro una request http para validar el bearer token incluido en su cabecera
        //Retorna true si posee token de acceso valido, de lo contrario retorna false
        public static async Task<ClaimsPrincipal> Validate(HttpRequest request)
        {
            Console.WriteLine("Debe autenticar",  Environment.GetEnvironmentVariable("mustBeAuthenticated", EnvironmentVariableTarget.Process));
            if (!MustBeAuthenticated())
                return new ClaimsPrincipal();
            string accessToken;
            ClaimsPrincipal authorize;
            IAuthentication auth = new Authentication(
                Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process),
                Environment.GetEnvironmentVariable("tenant", EnvironmentVariableTarget.Process),
                Environment.GetEnvironmentVariable("tenantID", EnvironmentVariableTarget.Process)
            );
            //Console.WriteLine("El Token que recibo:");
            //Console.WriteLine(GetAccessToken(request));
            //Console.WriteLine("|-----------------|");
            if ((accessToken = GetAccessToken(request)) != null) {
                authorize = await auth.ValidateAccessToken(accessToken);
                if (authorize != null)
                    return authorize;
            }
            return null;
        }
        private static string GetAccessToken(HttpRequest req)
        {
            var authorizationHeader = req.Headers?["Authorization"];
            string[] parts = authorizationHeader?.ToString().Split(null) ?? new string[0];
            if (parts.Length == 2 && parts[0].Equals("Bearer"))
                return parts[1];
            return null;
        }

    }
}