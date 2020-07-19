using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using trifenix.agro.authentication.interfaces;

namespace trifenix.agro.authentication.operations {
    public class Authentication : IAuthentication {

        private readonly string _clientID;
        private readonly string _tenant;
        private readonly string _tenantID;
        private readonly string[] _validAudiences;

        //Usuario test: cristian.rojas@alumnos.uv.cl
        //Password:     Tricking4life
        public Authentication(string clientID, string tenant, string tenantID, string[] validAudiences) {
            _clientID = clientID;     //Id aplicacion registrada en Azure Active Directory      //a81f0ad4-912b-46d3-ba3e-7bf605693242
            _tenant = tenant;         //Nombre inquilino en Azure Active Directory              //jhmad.onmicrosoft.com
            _tenantID = tenantID;     //Id inquilino en Azure Active Directory                  //dc17aef1-b155-4005-aa00-9e80f52d2a7d
            _validAudiences = validAudiences;   //Dominios permitidos para solicitar el token al inquilino  //new[] { "https://aresa.trifenix.io", "https://dev-aresa.trifenix.io" }
        }

        public async Task<ClaimsPrincipal> ValidateAccessToken(string accessToken) {
            string aadInstance = "https://sts.windows.net/{0}/";
            string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, _tenant);
            List<string> validIssuers = new List<string>() {
                //$"https://login.microsoftonline.com/{_tenant}/",
                //$"https://login.microsoftonline.com/{_tenant}/v2.0",
                //$"https://login.microsoftonline.com/{_tenantID}/",
                $"https://login.microsoftonline.com/{_tenantID}/v2.0",
                //$"https://login.windows.net/{_tenant}/",
                //$"https://login.microsoft.com/{_tenant}/",
                $"https://sts.windows.net/{_tenantID}/"
            };
            ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{authority}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
            OpenIdConnectConfiguration config = null;
            config = await configManager.GetConfigurationAsync();
            ISecurityTokenValidator tokenValidator = new JwtSecurityTokenHandler();
            // Initialize the token validation parameters
            TokenValidationParameters validationParameters = new TokenValidationParameters {
                // App Id URI and AppId of this service application are both valid audiences.
                ValidAudiences = _validAudiences,
                ValidIssuers = validIssuers,
                IssuerSigningKeys = config.SigningKeys,
                ValidateIssuer = false
            };
            try {
                SecurityToken securityToken;
                var claimsPrincipal = tokenValidator.ValidateToken(accessToken, validationParameters, out securityToken);
                return claimsPrincipal;
            }
            catch (Exception ex) {
                Console.WriteLine("Error in catch: \n|--------------------------------------------------------------------------------------------------------|");
                Console.WriteLine(ex.Message);
                Console.WriteLine("|--------------------------------------------------------------------------------------------------------|");
            }
            return null;
        }

    }
}