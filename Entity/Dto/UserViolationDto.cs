using Entity.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entity.Dto
{
    public class UserViolationDto
    {
        public int Id { get; set; }
        public DateTime DateViolation { get; set; }
        public decimal FineValue { get; set; }
        public string State { get; set; }

        // Clave for�nea hacia Payment
        public int? PaymentId { get; set; }  // Nullable, porque no todas las violaciones tienen pago
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }

        // Clave for�nea hacia Person
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
    }
}
