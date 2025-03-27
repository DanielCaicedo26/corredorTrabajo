public class InformationTypeInfraction
{
    public int Id { get; set; }
    public int TypeInfractionId { get; set; }
    public int InformationInfractionId { get; set; }
    public TypeInfraction TypeInfraction { get; set; }
    public InformationInfraction InformationInfraction { get; set; }
}
