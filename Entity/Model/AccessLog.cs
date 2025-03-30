using System;

namespace Entity.Model { 
public class AccessLog
{
    public int Id { get; set; }
    public  string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public string Details { get; set; }


}
}