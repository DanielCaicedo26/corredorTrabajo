using System;
using System.Collections.Generic;

public class Rol
{
    public int Id { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public ICollection<RolUser> RolUsers { get; set; }

    public Rol()
    {
        RolUsers = new List<RolUser>();
    }
}