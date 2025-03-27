public class Payment
{
    public int Id { get; set; }
    public string TypePayment { get; set; }
    public int UserPaymentId { get; set; }
    public UserViolation UserViolation { get; set; }
}
