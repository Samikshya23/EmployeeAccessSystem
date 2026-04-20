using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IFortigateItemRepositories
    {
        Task<IEnumerable<FortigateItem>> GetAllAsync();
        Task<IEnumerable<FortigateItem>> GetActiveAsync();
        Task<IEnumerable<FortigateItem>> GetByCategoryAsync(int fortigateCategoryId);
        Task<FortigateItem> GetByIdAsync(int id);
        Task<int> AddAsync(FortigateItem model);
        Task<int> UpdateAsync(FortigateItem model);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleAsync(int id);
    }
}