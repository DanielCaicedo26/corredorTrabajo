using System;
using System.Collections.Generic;

namespace Entity.Model
{
    public class Person
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