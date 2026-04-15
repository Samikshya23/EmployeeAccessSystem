using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IPingProductFieldRepository
    {
        Task<IEnumerable<PingProductField>> GetAllAsync();
        Task<IEnumerable<PingProductField>> GetActiveAsync();
        Task<IEnumerable<PingProductField>> GetByPingProductIdAsync(int pingProductId);
        Task<PingProductField> GetByIdAsync(int id);
        Task<PingProductField> CheckDuplicateAsync(int pingProductId, string fieldName);
        Task<PingProductField> CheckDuplicateForUpdateAsync(int pingProductFieldId, int pingProductId, string fieldName);
        Task<int> AddAsync(PingProductField pingProductField);
        Task<int> UpdateAsync(PingProductField pingProductField);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleAsync(int id);
    }
}