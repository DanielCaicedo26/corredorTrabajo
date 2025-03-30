using System;
using System.Collections.Generic;

namespace Entity.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int IdUserNotification { get; set; }
    }
}


