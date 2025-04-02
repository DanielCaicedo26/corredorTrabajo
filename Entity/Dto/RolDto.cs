using Entity.Model;

namespace Entity.Dto
{
    public class RolDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        // Relación con RolFormPermission
        public virtual ICollection<RolFormPermission> RolFormPermissions { get; set; } = new List<RolFormPermission>();

        // Relación con RolUser (Usuarios asociados a este Rol)
        public virtual ICollection<RolUser> RolUsers { get; set; } = new List<RolUser>();
    }
}
