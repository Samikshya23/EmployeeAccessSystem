using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface ISMCProductItemRepositories
    {
        Task<IEnumerable<SMCProductItem>> GetAllAsync();
        Task<IEnumerable<SMCProductItem>> GetActiveAsync();
        Task<IEnumerable<SMCProductItem>> GetByProductAsync(int smcProductId);
        Task<SMCProductItem> GetByIdAsync(int id);
        Task<int> AddAsync(SMCProductItem model);
        Task<int> UpdateAsync(SMCProductItem model);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleAsync(int id);
    }
}