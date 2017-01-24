using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityServer3.AccessTokenValidation;
using System.IdentityModel.Tokens;

namespace AngularAt
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {

            //
            // NuGet: IdentityServer3.AccessTokenValidation
            //
            var issuer = "https://steyer-identity-server.azurewebsites.net/identity";
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = issuer
            });

            // Claims-Mapping von OIDC-Claims auf SAML-Claims verhindern
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
        }
    }
}