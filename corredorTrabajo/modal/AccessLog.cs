using System;

public class AccessLog
{
    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public string Details { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
