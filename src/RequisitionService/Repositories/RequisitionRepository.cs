using RequisitionService.Interfaces;
using RequisitionService.Entities;
using RequisitionService.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RequisitionService.Repositories
{
    public class RequisitionRepository: IRequisitionRepository
    {
        private readonly AppDbContext _context;

        public RequisitionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Requisition>> GetAllAsync()
        {
            return await _context.Requisitions.ToListAsync();
        }

        public async Task<Requisition> GetAsync(Guid id)
        {
            return await _context.Requisitions.FindAsync(id);
        }
        
        public async Task<Requisition> GetAsync(Expression<Func<Requisition, bool>> filter)
        {
            return await _context.Requisitions.FirstOrDefaultAsync(filter);
        }

        public async Task CreateAsync(Requisition requisition)
        {
            _context.Add(requisition);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Requisition requisition)
        {
            _context.Update(requisition);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var requisition = await _context.Requisitions.FindAsync(id);

            _context.Remove(requisition);

            await _context.SaveChangesAsync();
        }
    }
}