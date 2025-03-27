using System.Collections.Generic;

public class ModuloForm
{
    public int Id { get; set; }
    public int RolPermissionId { get; set; }
    public int FormId { get; set; }
    public int ModuleId { get; set; }
    public Form Form { get; set; }
    public Module Module { get; set; }
}
