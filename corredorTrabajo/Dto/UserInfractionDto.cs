public class UserInfractionDto
{
    public int Id { get; set; }
    public int TypeInfractionId { get; set; }
    public TypeInfraccionDto TypeInfraction { get; set; }
}
