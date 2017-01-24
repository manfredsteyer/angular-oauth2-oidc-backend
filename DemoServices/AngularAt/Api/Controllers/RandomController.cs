using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class RandomController : ApiController
    {
        public string Get(int length = 100)
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[length];
            random.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}