using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace GO
{
 public class Passagier : Person
 {
  
  public string PassagierStatus { get; set; }

  public virtual ICollection<Flug> FlugSet { get; set; }
 }
}
