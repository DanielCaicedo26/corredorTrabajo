using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    public class TypeInfraction
    {
        public int Id { get; set; }

        
        public string TypeViolation { get; set; }  // Correg� la nomenclatura a PascalCase

        
        public string Description { get; set; }

        
        public decimal ValueViolation { get; set; }

        
        public int UserNotificationId { get; set; }  

        public virtual UserNotification UserNotification { get; set; } // Relaci�n con UserNotification
    }
}

