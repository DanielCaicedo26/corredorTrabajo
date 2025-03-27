using System.Collections.Generic;

public class TypeInfraction
{
    public int Id { get; set; }
    public string TypeViolation { get; set; }
    public string Description { get; set; }
    public decimal ValueViolation { get; set; }
    public int UserNotificationId { get; set; }
    public ICollection<UserInfraction> UserInfractions { get; set; }

    public TypeInfraction()
    {
        UserInfractions = new List<UserInfraction>();
    }
}
