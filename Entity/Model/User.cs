namespace Entity.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool NotificationsEnabled { get; set; } // Cambio a booleano y mejor nombre

        // Relación con RolUser (Roles asociados a este Usuario)
        public virtual ICollection<RolUser> RolUsers { get; set; } = new List<RolUser>();
    }
}

