using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IFortigateItemService
    {
        Task<IEnumerable<FortigateItem>> GetAllAsync();
        Task<IEnumerable<FortigateItem>> GetActiveAsync();
        Task<IEnumerable<FortigateItem>> GetByCategoryAsync(int fortigateCategoryId);
        Task<FortigateItem> GetByIdAsync(int id);
        Task<string> AddAsync(FortigateItem model);
        Task<string> UpdateAsync(FortigateItem model);
        Task<string> DeleteAsync(int id);
        Task<string> ToggleAsync(int id);
    }
}