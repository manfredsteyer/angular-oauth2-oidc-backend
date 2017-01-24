using GO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi_Demo;

namespace SVC.Controllers
{
    // /api/passagier
    [Authorize]
    [RoutePrefix("api/securepassagier")]
    public class SecurePassagierController : ApiController
    {
        private static List<Passagier> passagiere = new List<Passagier>();

        public SecurePassagierController()
        {
            if (passagiere.Count == 0)
            {
                passagiere.Add(new Passagier { Id = 1, Vorname = "Max", Name = "Muster", Geburtsdatum = new DateTime(1970, 1,1), PassagierStatus = "A" });
                passagiere.Add(new Passagier { Id = 2, Vorname = "Susi", Name = "Sorglos", Geburtsdatum = new DateTime(1971, 1, 1), PassagierStatus = "B" });
                passagiere.Add(new Passagier { Id = 3, Vorname = "Anna", Name = "Muster", Geburtsdatum = new DateTime(1972, 1, 1), PassagierStatus = "C" });
                passagiere.Add(new Passagier { Id = 4, Vorname = "Rainer", Name = "Zufall", Geburtsdatum = new DateTime(1973, 1, 1), PassagierStatus = "A" });
            }
        }

        public List<Passagier> GetAll() {
            return passagiere;
        }

        [Route("odata")]
        [CustomEnableQuery]
        public IQueryable<Passagier> GetOData() {
            return passagiere.AsQueryable();
        }

        // GET /api/passagier?pNummer=1
        public Passagier GetByNummer(int pNummer)
        {
            return passagiere.Where(p => p.Id == pNummer).FirstOrDefault();
        }

        // GET /api/passagier?name=Richter
        public List<Passagier> GetByName(string name)
        {
            return passagiere.Where(p => p.Name == name).ToList();
        }

        private static object sync = new object();

        private static String[] allowedFirstNames = { "Anna", "Rainer", "Ursula", "Karin", "Helga", "Sabine", "Susi", "Peter", "Michael", "Thomas", "Andreas", "Max" };

        private static String[] allowedNames = { "Muster", "Sorglos", "Zufall", "Müller", "Schmidt", "Schneider", "Fischer", "Weber", "Meyer", "Wagner", "Becker", "Schulz" };

        // POST /api/passagier
        public HttpResponseMessage PostPassagier(Passagier passagier)
        {
            if (passagier.Id <= 4 && passagier.Id >= 1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Datensätze mit Ids 1 bis 4 dürfen nicht geändert werden!") });
            }

            if (!allowedFirstNames.Contains(passagier.Vorname))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Es sind nur die folgenden Vornamen erlaubt: " + allowedFirstNames.OrderBy(s=>s).Aggregate((a, b) => a + ", " + b)) });
            }

            if (!allowedNames.Contains(passagier.Name))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Es sind nur die folgenden Nachnamen erlaubt: " + allowedNames.OrderBy(s=>s).Aggregate((a, b) => a + ", " + b)) });
            }

            
            lock (sync)
            {
                if (passagier.Id == 0)
                {
                    passagier.Id = passagiere.Max(p => p.Id) + 1;
                    passagiere.Add(passagier);

                    if (passagiere.Count > 1000)
                    {
                        var temp = passagiere.Take(4).ToList();
                        temp.AddRange(passagiere.Skip(500));
                        passagiere = temp;
                    }

                    return Request.CreateResponse(HttpStatusCode.Created, passagier);
                }

                var passagierInDb = passagiere.FirstOrDefault(p => p.Id == passagier.Id);

                if (passagierInDb == null)
                {
                    var error = "Der Eintrag mit der Id " + passagierInDb.Id + " wurde nicht gefunden. Lassen Sie die Id weg, um einen neuen Eintrag zu erzeugen.";
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(error) });
                }


                passagierInDb.Vorname = passagier.Vorname;
                passagierInDb.Name = passagier.Name;
                passagierInDb.Geburtsdatum = passagier.Geburtsdatum;
                passagierInDb.PassagierStatus = passagier.PassagierStatus;

                return Request.CreateResponse(HttpStatusCode.OK, passagier);
            }
        }


    }
}
