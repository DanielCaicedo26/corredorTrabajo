using System.Collections.Generic;

namespace Entity.Model
{
    public class TypeInfraction
    {
        public int Id { get; set; }
        public string typeviolation { get; set; }
        public string description { get; set; }
        public decimal ValueViolation { get; set; }
        public int UserNotificationId { get; set; }

    }
}


