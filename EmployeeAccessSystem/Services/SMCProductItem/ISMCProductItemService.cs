using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface ISMCProductItemService
    {
        Task<IEnumerable<SMCProductItem>> GetAllAsync();
        Task<IEnumerable<SMCProductItem>> GetByProductAsync(int smcProductId);
        Task<SMCProductItem> GetByIdAsync(int id);
        Task<string> AddAsync(SMCProductItem model);
        Task<string> UpdateAsync(SMCProductItem model);
        Task<string> DeleteAsync(int id);
        Task<string> ToggleAsync(int id);
    }
}