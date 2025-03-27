using System;
using System.Collections.Generic;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int IdAccessLog { get; set; }
    public int IdPerson { get; set; }
    public int IdUserNotification { get; set; }
    public ICollection<RolUser> RolUsers { get; set; }

    public User()
    {
        RolUsers = new List<RolUser>();
    }
}
