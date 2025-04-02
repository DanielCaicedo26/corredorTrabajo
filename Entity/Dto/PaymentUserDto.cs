using Entity.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Dto
{
    public class PaymentUserDto
    {
        public int Id { get; set; }
        public string PaymentAgreement { get; set; }
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
    }
}
