using Entity.Model;

namespace Entity.Dto
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }

        // Relación con UserViolation
        public virtual ICollection<UserViolation> UserViolations { get; set; } = new List<UserViolation>();

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
