using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface ISMCProductService
    {
        Task<IEnumerable<SMCProduct>> GetAllAsync();
        Task<SMCProduct> GetByIdAsync(int id);
        Task<string> AddAsync(SMCProduct smcProduct);
        Task<string> UpdateAsync(SMCProduct smcProduct);
        Task DeleteAsync(int id);
    }
}