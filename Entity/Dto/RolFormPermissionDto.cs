using Entity.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Dto
{
    public class RolFormPermission
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int FormId { get; set; }
        public int PermissionId { get; set; }
        [ForeignKey("RolId")]
        public virtual Rol Rol { get; set; }
        [ForeignKey("FormId")]
        public virtual Form Form { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
