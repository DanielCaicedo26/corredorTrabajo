
using Entity.Model;

namespace Entity.Dto { 
public class PaymentDto
{
        public int Id { get; set; }
        public string TypePayment { get; set; }

        // Eliminamos UserViolationId, porque la relación se gestiona desde UserViolation
        public virtual UserViolation UserViolation { get; set; }
        public int UserViolationId { get; set; }
    }
}
