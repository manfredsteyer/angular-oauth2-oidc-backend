using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GO
{
 public class Person
 {
  public int Id { get; set; }
  public string Vorname { get; set; }
  public string Name { get; set; }
  public Nullable<DateTime> Geburtsdatum { get; set; }

  
 }
}
