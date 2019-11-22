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

namespace trifenix.agro.authentication.operations
{
    public class Authentication : IAuthentication
    {

        private readonly string _clientID;
        private readonly string _tenant;
        private readonly string _tenantID;

        //Usuario test: Chris@aresaTest.onmicrosoft.com
        //Password:     T1r2i3c4k5i6n7g8
        public Authentication(string clientID, string tenant, string tenantID)
        {
            _clientID = clientID;     //Id aplicacion registrada en Azure Active Directory      //247a3f6-7aa4-4399-8849-894e702ec793
            _tenant = tenant;                  //Nombre inquilino en Azure Active Directory     //aresaTest.onmicrosoft.com
            _tenantID = tenantID;     //Id inquilino en Azure Active Directory                  //a6df0226-dec3-4685-a6ee-d6e707c4b7be
        }

        public async Task<ClaimsPrincipal> ValidateAccessToken(string accessToken)
        {
            // rest of the values below can be left as is in most circumstances
            string aadInstance = "https://login.microsoftonline.com/{0}/v2.0";
            string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, _tenant);
            List<string> validIssuers = new List<string>()
                {
                    //$"https://login.microsoftonline.com/{_tenant}/",
                    //$"https://login.microsoftonline.com/{_tenant}/v2.0",
                    //$"https://login.microsoftonline.com/{_tenantID}/",
                    $"https://login.microsoftonline.com/{_tenantID}/v2.0"
                    //$"https://login.windows.net/{_tenant}/",
                    //$"https://login.microsoft.com/{_tenant}/",
                    //$"https://sts.windows.net/{_tenantID}/"
                };
            ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{authority}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever()
            );
            OpenIdConnectConfiguration config = null;
            config = await configManager.GetConfigurationAsync();
            ISecurityTokenValidator tokenValidator = new JwtSecurityTokenHandler();
            // Initialize the token validation parameters
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                // App Id URI and AppId of this service application are both valid audiences.
                ValidAudiences = new[] { _clientID },
                ValidateIssuer = true,
                // Support Azure AD V1 and V2 endpoints.
                ValidIssuers = validIssuers,
                IssuerSigningKeys = config.SigningKeys
            };
            try
            {
                SecurityToken securityToken;
                var claimsPrincipal = tokenValidator.ValidateToken(accessToken, validationParameters, out securityToken);
                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("|Error in catch: " +
                    "-------------------------------------------------------------------------------------------------------|");
                Console.WriteLine(ex.Message);
                Console.WriteLine("|--------------------------------------------------------------------------------------------------------|");
            }
            return null;
        }

    }
}