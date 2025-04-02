using System.Collections.Generic;

namespace Entity.Model
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        // Relación con ModuleForm
        public virtual ICollection<ModuleForm> ModuleForms { get; set; } = new List<ModuleForm>();
    }
}

