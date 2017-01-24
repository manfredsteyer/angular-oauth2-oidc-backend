﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;


namespace WebApplication1.Controllers
{
    [Authorize]
    public class VoucherController : ApiController
    {
        public string Post(int amount)
        {
            var identity = User.Identity as ClaimsIdentity;
            foreach (var claim in identity.Claims)
            {
                Debug.WriteLine(claim.Type + ": " + claim.Value);
            }

            return "Voucher for " + User.Identity.Name + " about EUR " + amount + ", Voucher-Code: " + Guid.NewGuid().ToString();
        }

    }
}