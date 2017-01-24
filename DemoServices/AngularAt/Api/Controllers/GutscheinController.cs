using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;


namespace WebApplication1.Controllers
{
    [Authorize]
    public class GutscheinController : ApiController
    {

        public string Post(int betrag)
        {
            var identity = User.Identity as ClaimsIdentity;
            foreach (var claim in identity.Claims)
            {
                Debug.WriteLine(claim.Type + ": " + claim.Value);
            }

            return "Gutschein für " + User.Identity.Name + " über " + betrag + ", Gutscheincode: " + Guid.NewGuid().ToString();
        }

    }
}