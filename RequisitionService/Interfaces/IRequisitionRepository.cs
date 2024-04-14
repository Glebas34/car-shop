using RequisitionService.Entities;
using System.Linq.Expressions;

namespace RequisitionService.Interfaces
{
    public interface IRequisitionRepository
    {
        Task<List<Requisition>> GetAllAsync();
        Task<Requisition> GetAsync(Guid id);
        Task<Requisition> GetAsync(Expression<Func<Requisition, bool>> filter);
        Task CreateAsync(Requisition requisition);
        Task UpdateAsync(Requisition updatedRequisition);
        Task DeleteAsync(Guid id);
    }
}