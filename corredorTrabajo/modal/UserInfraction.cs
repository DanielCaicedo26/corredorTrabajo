public class UserInfraction
{
    public int Id { get; set; }
    public int TypeInfractionId { get; set; }
    public TypeInfraction TypeInfraction { get; set; }
}
