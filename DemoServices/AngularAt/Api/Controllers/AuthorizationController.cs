using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;

namespace WebApplication1.Controllers
{
    public class AuthorizationController : Controller
    {
        // GET: /authorization
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username = null, string password = null)
        {
            var claims = new List<System.Security.Claims.Claim>();

            claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimsIdentity.DefaultNameClaimType, username));
            claims.Add(new System.Security.Claims.Claim("sub", username));
            claims.Add(new System.Security.Claims.Claim("user_id", "4711"));
            claims.Add(new System.Security.Claims.Claim("Role", "Manager"));
            claims.Add(new System.Security.Claims.Claim("Company", "ACME"));
            claims.Add(new System.Security.Claims.Claim("aud", "test"));
            claims.Add(new System.Security.Claims.Claim("iss", "http://authsvc"));

            var auth = this.HttpContext.GetOwinContext().Authentication;

            // Auth-Endpoint
            auth.SignIn(new System.Security.Claims.ClaimsIdentity(claims, "Bearer"));

            // Rückgabewert ist nicht relevant, da OAuth-Middleware die Antwort ohnehin abfängt!
            return null;
        }
    }
}