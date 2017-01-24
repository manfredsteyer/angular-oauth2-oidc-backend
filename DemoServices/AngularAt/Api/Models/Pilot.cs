using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GO
{
 public class Pilot : Person
 {

  public Nullable<System.DateTime> Einstellungsdatum { get; set; }

  public virtual ICollection<Flug> FlugSet { get; set; }
 }
}
