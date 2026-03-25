using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface ISMCProductRepository
    {
        Task<IEnumerable<SMCProduct>> GetAllAsync();
        Task<SMCProduct> GetByIdAsync(int id);
        Task AddAsync(SMCProduct smcProduct);
        Task UpdateAsync(SMCProduct smcProduct);
        Task DeleteAsync(int id);
    }
}