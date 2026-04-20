using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IFortigateCategoryService
    {
        Task<IEnumerable<FortigateCategory>> GetAllAsync();
        Task<IEnumerable<FortigateCategory>> GetActiveAsync();
        Task<FortigateCategory> GetByIdAsync(int id);
        Task<string> AddAsync(FortigateCategory model);
        Task<string> UpdateAsync(FortigateCategory model);
        Task<string> DeleteAsync(int id);
        Task<string> ToggleAsync(int id);
    }
}