using Microsoft.EntityFrameworkCore;
using ImageService.Interfaces;
using ImageService.Database;
using ImageService.Entities;
using System.Linq.Expressions;

namespace ImageService.Repositories
{
    public class ImageRepository: IImageRepository
    {
        private readonly AppDbContext _context;

        public ImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Image> GetAsync(Guid id)
        {
            return await _context.Images.FirstOrDefaultAsync(c=>c.Id==id);
        }

        public async Task<List<Image>> GetAllAsync()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task<List<Image>> GetAllAsync(Expression<Func<Image, bool>> filter)
        {
            return await _context.Images.Where(filter).ToListAsync();
        }

        public async Task<Image> GetAsync(Expression<Func<Image, bool>> filter)
        {
            return await _context.Images.FirstOrDefaultAsync(filter);
        }

        public async Task CreateAsync(Image image)
        {
            _context.Add(image);
            
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Image image)
        {
            _context.Update(image);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id){
            var image = await _context.Images.FindAsync(id);

            _context.Remove(image);

            await _context.SaveChangesAsync();
        }
    }
}