using Entity.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Dto
{
    public class ModuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        // Relación con ModuleForm
        public virtual ICollection<ModuleForm> ModuleForms { get; set; } = new List<ModuleForm>();
    }

}


