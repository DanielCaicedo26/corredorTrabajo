using System;
using System.Collections.Generic;

public class RolUser
{
    public int Id { get; set; }
    public int RolId { get; set; }
    public int UserId { get; set; }
    public Rol Rol { get; set; }
    public User User { get; set; }
}
