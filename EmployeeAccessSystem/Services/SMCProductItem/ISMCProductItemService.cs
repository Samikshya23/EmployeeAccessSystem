using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface ISMCProductItemService
    {
        Task<IEnumerable<SMCProductItem>> GetAllAsync();
        Task<SMCProductItem> GetByIdAsync(int id);
        Task<string> AddAsync(SMCProductItem model);
        Task<string> UpdateAsync(SMCProductItem model);
        Task<string> DeleteAsync(int id);
        Task ToggleAsync(int id);
        Task<IEnumerable<SMCProductItem>> GetByProductAsync(int smcProductId);
    }
}