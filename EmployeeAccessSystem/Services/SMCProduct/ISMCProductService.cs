using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface ISMCProductService
    {
        Task<IEnumerable<SMCProduct>> GetAllAsync();
        Task<IEnumerable<SMCProduct>> GetActiveAsync();
        Task<SMCProduct> GetByIdAsync(int id);
        Task<string> AddAsync(SMCProduct smcProduct);
        Task<string> UpdateAsync(SMCProduct smcProduct);
        Task<string> DeleteAsync(int id);
        Task<string> ToggleAsync(int id);
    }
}