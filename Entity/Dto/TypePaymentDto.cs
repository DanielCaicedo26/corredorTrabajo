
namespace Entity.Dto
{
    public class TypePaymentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}