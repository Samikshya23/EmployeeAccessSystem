using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IFortigateCategoryRepository
    {
        Task<IEnumerable<FortigateCategory>> GetAllAsync();
        Task<IEnumerable<FortigateCategory>> GetActiveAsync();
        Task<FortigateCategory> GetByIdAsync(int id);
        Task<int> AddAsync(FortigateCategory model);
        Task<int> UpdateAsync(FortigateCategory model);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleAsync(int id);
    }
}