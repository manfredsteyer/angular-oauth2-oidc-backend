using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web;


namespace FirstSteps
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            var clients = new List<Client>();
            
            clients.Add(new Client
            {
                Enabled = true,
                ClientName = "spa-demo-client",
                ClientId = "spa-demo",
                AllowRememberConsent = true,
                RedirectUris = new List<string> {
                    "http://localhost:8080/index.html",
                    "http://localhost:8080/home",
                },
                Flow = Flows.Implicit,
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:8080"
                },
                AllowAccessToAllScopes = true
            });

            clients.Add(new Client
            {
                Enabled = true,
                ClientName = "DemoClientResourceOwner",
                ClientId = "demo-resource-owner",
                ClientSecrets = new List<Secret>
                {
                    new Secret("geheim".Sha256())
                },
                Flow = Flows.ResourceOwner,
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:8080"
                },
                AllowAccessToAllScopes = true
            });

            var users = new List<InMemoryUser>();

            users.Add(new InMemoryUser
            {
                Username = "max",
                Password = "geheim",
                Subject = "4711",
                // Provider = "Facebook",
                // ProviderId = "10206078768620483",
                Claims = new List<Claim>
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Max"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Muster"),
                    new Claim(Constants.ClaimTypes.Email, "max@acme.com"),
                    new Claim(Constants.ClaimTypes.EmailVerified, "true"),
                    new Claim("projects", "A,B,C"),
                    new Claim("role", "Premium-Customer"),
                    new Claim("buyInBulk", "true")
                }
            });

            var scopes = new List<Scope>();

            scopes.Add(new Scope
            {
                Enabled = true,
                Name = "voucher",
                DisplayName = "May buy vouchers",
                Description = "",
                Type = ScopeType.Resource,
                IncludeAllClaimsForUser = true
            });

            scopes.AddRange(StandardScopes.All); // openid profile email

            foreach(var scope in scopes) {
                foreach (var scopeClaim in scope.Claims)
                {
                    scopeClaim.AlwaysIncludeInIdToken = true;
                }
            }


            var factory = new IdentityServerServiceFactory()
                            .UseInMemoryClients(clients)
                            .UseInMemoryScopes(scopes)
                            .UseInMemoryUsers(users);

            

    app.Map("/identity", idsrvApp =>
    {
        idsrvApp.UseIdentityServer(new IdentityServerOptions
        {
            SiteName = "IdentityServer3",
            SigningCertificate = LoadCertificate(),
            Factory = factory,
            RequireSsl = false
            /*
            
            // Login via Facebook
            AuthenticationOptions = new AuthenticationOptions
            {
                IdentityProviders = ConfigureIdentityProviders
                //                      ^
                //                      +---- Funktion für Konfiguration
            },
            */
        });
    });
}

        X509Certificate2 LoadCertificate() {
            return this.LoadFileCertificate();
        }

        X509Certificate2 LoadFileCertificate()
        {
            //
            // Das Laden von private Keys aus dem Dateisystem
            // findet hier zur Vereinfachung des Beispiels statt.
            // Bei einer Produktivanwendung sollten
            // die Keys *IMMER* aus dem geschützten Cert-Store
            // von Windows geladen werden
            //

            var flags = X509KeyStorageFlags.MachineKeySet
                            | X509KeyStorageFlags.PersistKeySet
                            | X509KeyStorageFlags.Exportable;
            
            var fileName = string.Format(@"{0}bin\ca.pfx", AppDomain.CurrentDomain.BaseDirectory);
            
            var file = File.ReadAllBytes(fileName);
            return new X509Certificate2(fileName, "pwd", flags);

            /*
                // Cert auf Kommandozeile erstellen
                makecert.exe -n "CN=IdentityCARoot" -r -pe -a sha512 -len 4096 -cy authority -sv IdentityCARoot.pvk IdentityCARoot.cer 
                pvk2pfx.exe -pvk IdentityCARoot.pvk -spc IdentityCARoot.cer -pfx IdentityCARoot.pfx -po pwd 
            */
        }

        public static void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {
                AppId = "108594322925470",
                AppSecret = "630cd556073b7308671020390109fda3",
                Caption = "Facebook",
                SignInAsAuthenticationType = signInAsType
            });
            
        }
}
}
