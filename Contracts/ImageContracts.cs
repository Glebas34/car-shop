namespace Contracts
{
    public record ImageCreated
    {
        public Guid Id {get;init;}
        public string Url {get;init;}
        public Guid CarPageId {get; init;}
        public bool IsMain {get; init;}
    }
    
    public record ImageDeleted
    {
        public Guid Id {get;init;}
        public bool IsMain {get; init;}
        public Guid CarPageId {get; init;}
        public string Url {get;init;}
    }
}
