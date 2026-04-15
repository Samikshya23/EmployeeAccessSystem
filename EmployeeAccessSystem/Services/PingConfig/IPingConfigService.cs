using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IPingConfigService
    {
        Task<IEnumerable<PingConfig>> GetAllAsync();
        Task<PingConfig?> GetByIdAsync(int id);
        Task<PingConfig?> GetExistingAsync(int productId, int pingProductId, int itemId, DateTime date);
        Task<string> AddAsync(PingConfig model, string? currentUser);
        Task<string> UpdateAsync(PingConfig model, string? currentUser);
        Task<string> DeleteAsync(int id, string? currentUser);
        Task<string> ToggleAsync(int id);
    }
}