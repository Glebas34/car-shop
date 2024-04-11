namespace RequisitionService.Dtos
{
    public record CreateRequisition
    {
        public string FullName {get;set;}
        public string PhoneNumber {get;set;}
        public Guid CarPageId {get; init;}
    }
}