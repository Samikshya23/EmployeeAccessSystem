using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IPingProductItemValueService
    {
        Task<IEnumerable<PingProductItemValue>> GetAllAsync();
        Task<IEnumerable<PingProductItemValue>> GetByItemIdAsync(int pingProductItemId);
        Task<PingProductItemValue> GetByIdAsync(int id);
        Task<string> AddAsync(PingProductItemValue pingProductItemValue);
        Task<string> UpdateAsync(PingProductItemValue pingProductItemValue);
        Task<string> DeleteAsync(int id);
    }
}