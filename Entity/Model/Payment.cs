namespace Entity.Model
{

    public class Payment
{
    public int Id { get; set; }
    public string TypePayment { get; set; }
    public int UserPaymentid { get; set; }
        public int UserViolationId { get; set; }
    }
}
