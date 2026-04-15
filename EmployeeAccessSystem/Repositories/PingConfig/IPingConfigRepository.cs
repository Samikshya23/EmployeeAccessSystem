using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IPingConfigRepository
    {
        Task<IEnumerable<PingConfig>> GetAllAsync();
        Task<PingConfig?> GetByIdAsync(int id);
        Task<PingConfig?> GetExistingAsync(int productId, int pingProductId, int itemId, DateTime date);
        Task<int> AddAsync(PingConfig model);
        Task<int> UpdateAsync(PingConfig model);
        Task<int> DeleteAsync(int id, DateTime? deletedDate, string? deletedBy);
        Task<int> ToggleAsync(int id);
    }
}