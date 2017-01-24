using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SVC.Controllers;
using System.IO;
using System.Web;

namespace WWWings_WebAPI.Controllers
{

    public class BuchungController : ApiController
    {
        private static List<Buchung> buchungen = new List<Buchung>();

        private static object sync = new object();

        public void Post(Buchung buchung)
        {
            //// Achtung! Auf Zugriffsrechte im Dateisystem achten !!

            //var fileName = HttpContext.Current.Server.MapPath("~/buchungen.txt");
            //var record = "Flug=" + buchung.FlugID + ", Passagier=" + buchung.PassagierID;

            //File.AppendAllText(fileName, record);

            lock (sync)
            {
                buchungen.Add(buchung);

                if (buchungen.Count > 1000)
                {
                    buchungen = buchungen.Skip(500).ToList();
                }
            }
        }

        public List<Buchung> Get()
        {
            return buchungen;
        }
    }
}
