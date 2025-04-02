using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    public class RolUser
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("RolId")]
        public virtual Rol Rol { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
