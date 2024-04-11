using Microsoft.EntityFrameworkCore;
using CardService.Interfaces;
using CardService.Database;
using CardService.Entities;
using System.Linq.Expressions;

namespace CardService.Repositories
{
    public class CardRepository: ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Card> GetAsync(Guid id)
        {
            return await _context.Cards.FirstOrDefaultAsync(c=>c.Id==id);
        }

        public async Task<Card> GetAsync(Expression<Func<Card, bool>> filter)
        {
            return await _context.Cards.FirstOrDefaultAsync(filter);
        }

        public async Task<List<Card>> GetAllAsync()
        {
            return await _context.Cards.ToListAsync();
        }
        public async Task<List<Card>> GetAllAsync(Expression<Func<Card, bool>> filter)
        {
            return await _context.Cards.Where(filter).ToListAsync();
        }

        public async Task CreateAsync(Card card)
        {
            _context.Add(card);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Card card)
        {
            _context.Update(card);
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c=>c.Id==id);

            _context.Remove(card);

            await _context.SaveChangesAsync();
        }
    }
}