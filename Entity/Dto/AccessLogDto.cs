using System;

public class AccessLogDto
{
    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public string Details { get; set; }
}
