using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entity.Model
{
    public class Permission
    {
        public int Id { get; set; }

  
        public string Name { get; set; }

        
        public string Description { get; set; }

        // Relación con RolFormPermission
        public virtual ICollection<RolFormPermission> RolFormPermissions { get; set; } = new List<RolFormPermission>();
    }
}
