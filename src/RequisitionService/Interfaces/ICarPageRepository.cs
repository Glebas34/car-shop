using RequisitionService.Entities;
namespace RequisitionService.Interfaces 
{
    public interface ICarPageRepository
    {
        Task<CarPage> GetAsync(Guid id);
        Task<List<CarPage>> GetAllAsync();
        Task CreateAsync(CarPage carPage);
        Task UpdateAsync(CarPage carPage);
        Task DeleteAsync(Guid id);
    }
}