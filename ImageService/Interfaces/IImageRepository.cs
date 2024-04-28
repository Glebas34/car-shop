using ImageService.Entities;
using System.Linq.Expressions;

namespace ImageService.Interfaces 
{
    public interface IImageRepository
    {
        Task<Image> GetAsync(Guid id);
        Task<List<Image>> GetAllAsync();
        Task CreateAsync(Image image);
        Task UpdateAsync(Image image);
        Task DeleteAsync(Guid id);
        Task<List<Image>> GetAllAsync(Expression<Func<Image, bool>> filter);
        Task<Image> GetAsync(Expression<Func<Image, bool>> filter);
    }
}