namespace Entity.Dto
{
    public class InformationInfractionDto
    {

        public int Id { get; set; }
        public int NumberSMLDV { get; set; }  // Debe ser int
        public decimal MinimumWage { get; set; }  // Debe ser decimal
        public decimal ValueSMLDV { get; set; }
        public decimal TotalValue { get; set; }
    }
}


