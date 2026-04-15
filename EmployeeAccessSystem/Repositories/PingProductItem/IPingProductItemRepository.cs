using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IPingProductItemRepository
    {
        Task<IEnumerable<PingProductItem>> GetAllAsync();
        Task<IEnumerable<PingProductItem>> GetActiveAsync();
        Task<IEnumerable<PingProductItem>> GetByPingProductIdAsync(int pingProductId);
        Task<PingProductItem> GetByIdAsync(int id);
        Task<PingProductItem> CheckDuplicateAsync(int pingProductId, string itemName);
        Task<PingProductItem> CheckDuplicateForUpdateAsync(int pingProductItemId, int pingProductId, string itemName);
        Task<int> AddAsync(PingProductItem pingProductItem);
        Task<int> UpdateAsync(PingProductItem pingProductItem);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleAsync(int id);
    }
}