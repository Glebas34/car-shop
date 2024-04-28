using Firebase.Storage;
namespace ImageService.Entities
{
    public class Image
    {
        public Guid Id { get; init; }
        public string Url { get; init; }
        public Guid CarPageId { get; init; }
        public string Root { get; init; }
        public bool IsMain { get; init; }
    }
} 