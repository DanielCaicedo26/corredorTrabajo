
using Entity.Model;

namespace Entity.Dto
{
    public class TypeInfractionDto
    {
        public int Id { get; set; }


        public string TypeViolation { get; set; }  // Corregí la nomenclatura a PascalCase


        public string Description { get; set; }


        public decimal ValueViolation { get; set; }


        public int UserNotificationId { get; set; }

        public virtual UserNotification UserNotification { get; set; } // Relación con UserNotification
    }
}
