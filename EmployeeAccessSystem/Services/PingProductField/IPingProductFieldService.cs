using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IPingProductFieldService
    {
        Task<IEnumerable<PingProductField>> GetAllAsync();
        Task<IEnumerable<PingProductField>> GetActiveAsync();
        Task<PingProductField> GetByIdAsync(int id);
        Task<string> AddAsync(PingProductField pingProductField);
        Task<string> UpdateAsync(PingProductField pingProductField);
        Task<string> DeleteAsync(int id);
        Task<string> ToggleAsync(int id);
    }
}