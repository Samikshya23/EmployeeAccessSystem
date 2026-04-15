using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IPingProductItemService
    {
        Task<IEnumerable<PingProductItem>> GetAllAsync();
        Task<IEnumerable<PingProductItem>> GetActiveAsync();
        Task<PingProductItem> GetByIdAsync(int id);
        Task<string> AddAsync(PingProductItem pingProductItem);
        Task<string> UpdateAsync(PingProductItem pingProductItem);
        Task<string> DeleteAsync(int id);
        Task<string> ToggleAsync(int id);
    }
}