namespace CardService.Dtos
{
    public record CardDto
    {   
        public string Manufacturer {get; init;}
        public string Model {get;init;}
        public decimal Price {get;set;}
        public Guid CarPageId {get;init;}
        public string Package {get;set;}
        public string Color {get;set;}
        public int Year {get;set;}
        public string Image {get;set;}
    }
}