using System.Collections.Generic;

public class Module
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public ICollection<ModuloForm> ModuloForms { get; set; }

    public Module()
    {
        ModuloForms = new List<ModuloForm>();
    }
}
