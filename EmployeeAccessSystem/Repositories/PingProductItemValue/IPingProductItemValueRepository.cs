using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IPingProductItemValueRepository
    {
        Task<IEnumerable<PingProductItemValue>> GetAllAsync();
        Task<IEnumerable<PingProductItemValue>> GetByItemIdAsync(int pingProductItemId);
        Task<PingProductItemValue> GetByIdAsync(int id);
        Task<PingProductItemValue> CheckDuplicateAsync(int pingProductItemId, int pingProductFieldId);
        Task<int> AddAsync(PingProductItemValue pingProductItemValue);
        Task<int> UpdateAsync(PingProductItemValue pingProductItemValue);
        Task<int> DeleteAsync(int id);
    }
}