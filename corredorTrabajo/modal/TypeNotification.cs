using System.Collections.Generic;

public class TypeNotification
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<UserNotification> UserNotifications { get; set; }

    public TypeNotification()
    {
        UserNotifications = new List<UserNotification>();
    }
}
