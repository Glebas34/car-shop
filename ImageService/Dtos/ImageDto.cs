namespace ImageService.Dtos
{
    public record CreateImage
    {
        public IFormFile Image { get; init; }
        public Guid CarPageId { get; set; }
    }
}