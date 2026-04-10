using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface ISMCProductRepository
    {
        Task<IEnumerable<SMCProduct>> GetAllAsync();
        Task<IEnumerable<SMCProduct>> GetActiveAsync();
        Task<IEnumerable<SMCProduct>> GetByProductIdAsync(int productId);
        Task<SMCProduct> GetByIdAsync(int id);
        Task<SMCProduct> CheckDuplicateAsync(int productId, string smcProductName);
        Task<SMCProduct> CheckDuplicateForUpdateAsync(int smcProductId, int productId, string smcProductName);
        Task<int> AddAsync(SMCProduct smcProduct);
        Task<int> UpdateAsync(SMCProduct smcProduct);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleAsync(int id);
    }
}