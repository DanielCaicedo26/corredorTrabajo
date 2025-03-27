using System.Collections.Generic;

public class InformationType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<InformationTypeInfraction> InformationTypeInfractions { get; set; }

    public InformationType()
    {
        InformationTypeInfractions = new List<InformationTypeInfraction>();
    }
}
