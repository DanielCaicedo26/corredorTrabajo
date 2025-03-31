using System;
namespace Entity.Dto { 
public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
        public string Password { get; set; }
        public bool NotificationsEnabled { get; set; }
    }
}
