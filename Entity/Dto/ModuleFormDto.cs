using Entity.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Dto
{
    public class ModuleFormDto
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }  // Clave foránea correcta
        public int FormId { get; set; }    // Clave foránea correcta

        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }

        [ForeignKey("FormId")]
        public virtual Form Form { get; set; }
    }
}


