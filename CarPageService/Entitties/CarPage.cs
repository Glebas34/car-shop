namespace CarPageService.Entities
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
        public string MainImage {get;set;} = "https://firebasestorage.googleapis.com/v0/b/car-shop-e3217.appspot.com/o/CarImages%2Fdefault.jpg?alt=media&token=3a120b4c-30f8-4518-8444-c6e752b6e00b";
        public List<string> Images {get;set;}
    }
}