namespace CardService.Entities{
    public class CarPage
    {
        public Guid Id {get;init;}
        public string Manufacturer {get;set;}
        public string Model {get;set;}
        public decimal Price {get;set;}
        public string Package {get;set;}
        public string Color {get;set;}
        public int Year {get;set;}
    }
}