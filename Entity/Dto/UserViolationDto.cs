using System;
namespace Entity.Dto
{
    public class UserViolationDto
    {
        public int Id { get; set; }
        public DateTime DateViolation { get; set; }
        public decimal FineValue { get; set; }
        public string State { get; set; }
    }
}
