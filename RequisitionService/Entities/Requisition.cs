namespace RequisitionService.Entities
{
    public class Requisition
    {
        public Guid Id {get;init;}
        public string PhoneNumber {get; init;}
        public string FullName {get; init;}
        public string Manufacturer {get;set;}
        public string Model {get;set;}
        public int Year {get;set;}
        public decimal Price {get;set;}
        public string Color {get;set;}
        public string Package {get;set;}
        public Guid CarPageId {get;init;}
    }
}