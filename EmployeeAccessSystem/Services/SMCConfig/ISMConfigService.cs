using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface ISMCConfigService
    {
        Task<IEnumerable<SMCConfig>> GetAllAsync();
        Task<SMCConfig?> GetByIdAsync(int id);
        Task<SMCConfig?> GetExistingAsync(int productId, int smcProductId, int itemId, DateTime date);
        Task<string> AddAsync(SMCConfig model, string? currentUser);
        Task<string> UpdateAsync(SMCConfig model, string? currentUser);
        Task<string> DeleteAsync(int id, string? currentUser);
        Task<string> ToggleAsync(int id);
    }
}