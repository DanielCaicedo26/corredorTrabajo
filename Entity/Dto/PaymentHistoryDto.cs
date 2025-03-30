using System;
namespace Entity.Dto
{
    public class PaymentHistoryDto
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime DiscountDate { get; set; }
    }
}
