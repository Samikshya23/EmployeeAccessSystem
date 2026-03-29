using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface ISMCProductRepository
    {
        Task<IEnumerable<SMCProduct>> GetAllAsync();
        Task<IEnumerable<SMCProduct>> GetActiveAsync();
        Task<IEnumerable<SMCProduct>> GetByProductIdAsync(int productId);
        Task<SMCProduct> GetByIdAsync(int id);
        Task<int> AddAsync(SMCProduct smcProduct);
        Task<int> UpdateAsync(SMCProduct smcProduct);
        Task<int> DeleteAsync(int id);
        Task ToggleAsync(int id);
    }
}