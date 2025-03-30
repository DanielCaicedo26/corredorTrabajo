using System.Collections.Generic;

namespace Entity.Model
{
    public class RolFormPermission
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int FormId { get; set; }
        public int PermissionId { get; set; }
    }
}


