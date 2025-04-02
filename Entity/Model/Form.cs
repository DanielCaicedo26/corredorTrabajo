using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.Model
{
    public class Form
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }

        // Relación con ModuleForm
        public virtual ICollection<ModuleForm> ModuleForms { get; set; } = new List<ModuleForm>();

        // Relación con RolFormPermission
        public virtual ICollection<RolFormPermission> RolFormPermissions { get; set; } = new List<RolFormPermission>();
    }
}