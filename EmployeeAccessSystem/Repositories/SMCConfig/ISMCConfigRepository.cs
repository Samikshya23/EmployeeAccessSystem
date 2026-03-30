using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface ISMCConfigRepository
    {
        Task<IEnumerable<SMCConfig>> GetAllAsync();
        Task<SMCConfig?> GetByIdAsync(int id);
        Task<int> AddAsync(SMCConfig model);
        Task<int> UpdateAsync(SMCConfig model);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleAsync(int id);
        Task<SMCConfig?> GetExistingAsync(int productId, int smcProductId, int itemId, DateTime date);
    }
}