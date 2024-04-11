using CardService.Entities;
using System.Linq.Expressions;

namespace CardService.Interfaces 
{
    public interface ICardRepository
    {
        Task<Card> GetAsync(Guid id);
        Task<Card> GetAsync(Expression<Func<Card, bool>> filter);
        Task<List<Card>> GetAllAsync(Expression<Func<Card, bool>> filter);
        Task<List<Card>> GetAllAsync();
        Task CreateAsync(Card card);
        Task UpdateAsync(Card card);
        Task DeleteAsync(Guid id);
    }
}