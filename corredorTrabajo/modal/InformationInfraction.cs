using System.Collections.Generic;

public class InformationInfraction
{
    public int Id { get; set; }
    public int NumberSMLDV { get; set; }
    public decimal MinimumWage { get; set; }
    public decimal ValueSMLDV { get; set; }
    public decimal TotalValue { get; set; }
    public ICollection<InformationTypeInfraction> InformationTypeInfractions { get; set; }

    public InformationInfraction()
    {
        InformationTypeInfractions = new List<InformationTypeInfraction>();
    }
}
