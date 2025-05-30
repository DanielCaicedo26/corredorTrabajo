using Entity.Model;
using System;

namespace Entity.Dto
{
    public class FormDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }

        // Relaci�n con ModuleForm
        public virtual ICollection<ModuleForm> ModuleForms { get; set; } = new List<ModuleForm>();

        // Relaci�n con RolFormPermission
        public virtual ICollection<RolFormPermission> RolFormPermissions { get; set; } = new List<RolFormPermission>();
    }
}
