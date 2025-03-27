using System;
using System.Collections.Generic;

public class UserViolation
{
    public int Id { get; set; }
    public DateTime DateViolation { get; set; }
    public decimal FineValue { get; set; }
    public string State { get; set; }
    public int PaymentId { get; set; }
    public int PersonId { get; set; }
    public Payment Payment { get; set; }
    public Person Person { get; set; }
    public ICollection<PaymentHistory> PaymentHistories { get; set; }

    public UserViolation()
    {
        PaymentHistories = new List<PaymentHistory>();
    }
}
