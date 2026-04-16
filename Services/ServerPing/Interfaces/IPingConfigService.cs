using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IPingConfigService
    {
        Task<IEnumerable<PingConfig>> GetAllAsync();
        Task<PingConfig?> GetByIdAsync(int id);
        Task<string> AddAsync(PingConfig pingConfig, string currentUser);
        Task<string> UpdateAsync(PingConfig pingConfig, string currentUser);
        Task<string> DeleteAsync(int id, string currentUser);
    }
}