namespace Contracts
{
    public record CarPageCreated
    {
        public Guid Id {get;init;}
        public string Manufacturer {get;set;}
        public string Model {get;init;}
        public decimal Price {get;set;}
        public string Package {get;set;}
        public string Color {get;set;}
        public int Year {get;set;}
        public string Image {get;set;}
    }

    public record CarPageUpdated
    {
        public Guid Id {get;init;}
        public string Manufacturer {get;set;}
        public string Model {get;init;}
        public decimal Price {get;set;}
        public string Package {get;set;}
        public string Color {get;set;}
        public int Year {get;set;}
        public string Image {get;set;}
    }
    
    public record CarPageDeleted
    {
        public Guid Id {get;init;}
    }
}
