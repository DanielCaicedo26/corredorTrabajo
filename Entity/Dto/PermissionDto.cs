using Entity.Model;

namespace Entity.Dto
{
    public class PermissionDto
    {
        public int Id { get; set; }


        public string Name { get; set; }


        public string Description { get; set; }

        // Relación con RolFormPermission
        public virtual ICollection<RolFormPermission> RolFormPermissions { get; set; } = new List<RolFormPermission>();
    }
}
