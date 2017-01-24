using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GO
{

 [DataContract(Name = "flug")]
 public class Flug
 {

      public Flug()
      {
      }

     [DataMember(Name ="id")]
     public int Id { get; set; }

        [DataMember(Name = "abflugort")]
        public string Abflugort { get; set; }

        [DataMember(Name = "zielort")]
        public string Zielort { get; set; }

        [DataMember(Name = "datum")]
        public DateTime Datum { get; set; }

        [DataMember(Name = "plaetze")]
        public short Plaetze { get; set; }

        [DataMember(Name = "freiePlaetze")]
        public short FreiePlaetze { get; set; }

   
  


 }
}
