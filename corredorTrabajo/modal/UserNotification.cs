using System;

public class UserNotification
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime ShippingDate { get; set; }
    public string State { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int TypeNotificationId { get; set; }
    public TypeNotification TypeNotification { get; set; }
}
