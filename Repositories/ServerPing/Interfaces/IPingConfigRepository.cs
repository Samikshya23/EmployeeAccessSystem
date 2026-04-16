using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IPingConfigRepository
    {
        Task<IEnumerable<PingConfig>> GetAllAsync();
        Task<PingConfig?> GetByIdAsync(int id);
        Task<PingConfig?> CheckDuplicateAsync(int pingProductId, DateTime entryDate);
        Task<PingConfig?> CheckDuplicateForUpdateAsync(int pingConfigId, int pingProductId, DateTime entryDate);
        Task<int> AddAsync(PingConfig pingConfig);
        Task<int> UpdateAsync(PingConfig pingConfig);
        Task<int> DeleteAsync(int id, string deletedBy);
    }
}