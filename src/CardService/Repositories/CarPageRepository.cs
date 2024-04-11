using Microsoft.EntityFrameworkCore;
using CardService.Interfaces;
using CardService.Database;
using CardService.Entities;

namespace CardService.Repositories
{
    public class CarPageRepository: ICarPageRepository
    {
        private readonly AppDbContext _context;

        public CarPageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CarPage> GetAsync(Guid id)
        {
            return await _context.CarPages.FirstOrDefaultAsync(c=>c.Id==id);
        }

        public async Task<List<CarPage>> GetAllAsync()
        {
            return await _context.CarPages.ToListAsync();
        }

        public async Task CreateAsync(CarPage carPage)
        {
            _context.Add(carPage);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarPage carPage)
        {
            _context.Update(carPage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var carPage = await _context.CarPages.FirstOrDefaultAsync(c=>c.Id==id);
            _context.Remove(carPage);
            await _context.SaveChangesAsync();
        }
    }
}