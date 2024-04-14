namespace CarPageService.Dtos
{
    public record CarPageDto
    {
        public string Manufacturer {get;set;}
        public string Model {get;set;}
        public decimal Price {get;set;}
        public string Color {get;set;}
        public string Warranty {get;set;}
        public string Speed {get;set;}
        public string Power {get;set;}
        public string Acceleration {get;set;}
        public string FuelConsumption {get;set;}
        public string Package {get;set;}
        public int Year {get;set;}
    }

    public record CreateCarPage
    {
        public string Manufacturer {get;set;}
        public string Model {get;set;}
        public decimal Price {get;set;}
        public string Color {get;set;}
        public string Warranty {get;set;}
        public string Speed {get;set;}
        public string Power {get;set;}
        public string Acceleration {get;set;}
        public string FuelConsumption {get;set;}
        public string Package {get;set;}
        public int Year {get;set;}
    }

    public record UpdateCarPage
    {
        public Guid Id {get;init;}
        public string Manufacturer {get;set;}
        public string Model {get;set;}
        public string Color {get;set;}
        public decimal Price {get;set;}
        public string Package {get;set;}
        public string Warranty {get;set;}
        public string Speed {get;set;}
        public string Power {get;set;}
        public string Acceleration {get;set;}
        public string FuelConsumption {get;set;}
        public int Year {get;set;}
    }
}