using System;
using System.Collections.Generic;

namespace Entity.Model
{

    public class UserViolation
{
    public int Id { get; set; }
    public DateTime DateViolation { get; set; }
    public decimal FineValue { get; set; }
    public string State { get; set; }
    public int PaymentId { get; set; }
    public int PersonId { get; set; }
 
   
}
}
