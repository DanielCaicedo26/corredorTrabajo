using Entity.Model;
using System.ComponentModel.DataAnnotations.Schema;

public class ModuleForm
{
    public int Id { get; set; }

    public int ModuleId { get; set; }  // Clave for�nea correcta
    public int FormId { get; set; }    // Clave for�nea correcta

    [ForeignKey("ModuleId")]
    public virtual Module Module { get; set; }

    [ForeignKey("FormId")]
    public virtual Form Form { get; set; }
}
