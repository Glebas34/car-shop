namespace SeedingData.Entities
{
    public class CarPage
    {
        public Guid Id {get;init;}
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
        public string MainImage {get;set;}
        public List<string> Images {get;set;}
    }
}