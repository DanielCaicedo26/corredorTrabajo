using System;
using System.Collections.Generic;

public class Form
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateCreation { get; set; }
    public string Status { get; set; }
    public ICollection<RolFormPermission> RolFormPermissions { get; set; }

    public Form()
    {
        RolFormPermissions = new List<RolFormPermission>();
    }
}
