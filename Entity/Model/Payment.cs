using Entity.Model;

public class Payment
{
    public int Id { get; set; }
    public string TypePayment { get; set; }

    // Eliminamos UserViolationId, porque la relaci�n se gestiona desde UserViolation
    public virtual UserViolation UserViolation { get; set; }
    public object UserViolationId { get; set; }
}
