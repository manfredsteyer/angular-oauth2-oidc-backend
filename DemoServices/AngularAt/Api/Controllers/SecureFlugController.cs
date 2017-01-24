using GO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;

namespace WWWings_WebAPI.Controllers
{
    [Authorize]
    public class SecureFlugController : ApiController
    {
        private static List<Flug> fluege = new List<Flug>();

        public SecureFlugController()
        {
            if (fluege.Count == 0)
            {
                fluege.Add(new Flug { Id = 1, Abflugort = "Graz", Zielort ="Hamburg", Datum = DateTime.Now });
                fluege.Add(new Flug { Id = 2, Abflugort = "Graz", Zielort = "Hamburg", Datum = DateTime.Now.AddHours(2)});
                fluege.Add(new Flug { Id = 3, Abflugort = "Graz", Zielort = "Hamburg", Datum = DateTime.Now.AddHours(4) });
                fluege.Add(new Flug { Id = 4, Abflugort = "Graz", Zielort = "Mallorca", Datum = DateTime.Now }); 
            }
        }

        public HttpResponseMessage GetError(int statusCode)
        {
            return new HttpResponseMessage((HttpStatusCode)statusCode);
        }

        public Flug Get(int flugNummer)
        {
            return fluege.Where(f => f.Id == flugNummer).FirstOrDefault();
        }

        public List<Flug> Get()
        {
            var ci = User.Identity as ClaimsIdentity;

            foreach(var claim in ci.Claims)
            {
                Debug.WriteLine(claim.Type + ": " + claim.Value);
            }

            return fluege;
        }

        public List<Flug> Get(string abflugOrt, string zielOrt)
        {
            return fluege.Where(f => f.Abflugort == abflugOrt && f.Zielort == zielOrt).ToList();
        }
        
        private static object sync = new object();

        private static string[] allowed = { "Graz", "Wien", "München", "Hamburg", "Düsseldorf", "Zürich", "Berlin" };

        public HttpResponseMessage Post(Flug flug)
        {
            if (flug.Id <= 4 && flug.Id >= 1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Datensätze mit Ids 1 bis 4 dürfen nicht geändert werden!")});
            }

            if (!allowed.Contains(flug.Abflugort) || !allowed.Contains(flug.Zielort)) {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Es sind nur die folgenden Flughäfen erlaubt: " + allowed.OrderBy(s=>s).Aggregate((a, b) => a + ", " + b)) });
            }

            lock (sync)
            {

                if (flug.Id == 0)
                {
                    flug.Id = fluege.Max(f => f.Id) + 1;
                    fluege.Add(flug);

                    if (fluege.Count > 1000)
                    {
                        var temp = fluege.Take(4).ToList();
                        temp.AddRange(fluege.Skip(500));
                        fluege = temp;
                    }

                    return Request.CreateResponse(HttpStatusCode.Created, flug);
                }

                var flugInDb = Get(flug.Id);

                if (flugInDb == null)
                {
                    var error = "Der Eintrag mit der Id " + flug.Id + " wurde nicht gefunden. Lassen Sie die Id weg, um einen neuen Eintrag zu erzeugen.";
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(error) });
                }

                flugInDb.Datum = flug.Datum;
                flugInDb.Abflugort = flug.Abflugort;
                flugInDb.Zielort = flug.Zielort;
                flugInDb.Plaetze = flug.Plaetze;
                flugInDb.FreiePlaetze = flug.FreiePlaetze;


                return Request.CreateResponse(HttpStatusCode.OK, flug);
                   
            }
        }

    }
}
