using System;

public class PaymentHistory
{
    public int Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime DiscountDate { get; set; }
    public int UserViolationId { get; set; }
    public UserViolation UserViolation { get; set; }
}
