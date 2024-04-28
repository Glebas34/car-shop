using ImageService.Dtos;
using ImageService.Entities;
namespace ImageService.Interfaces
{
    public interface IStorageService
    {
        Task<Image> UploadAsync(CreateImage createImage, Guid carPageId, bool isMain);
        Task DeleteAsync(string root);
    }
}