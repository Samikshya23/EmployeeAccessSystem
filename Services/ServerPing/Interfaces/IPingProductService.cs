using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IPingProductService
    {
        Task<IEnumerable<PingProduct>> GetAllAsync();
        Task<IEnumerable<PingProduct>> GetActiveAsync();
        Task<PingProduct> GetByIdAsync(int id);
        Task<string> AddAsync(PingProduct pingProduct);
        Task<string> UpdateAsync(PingProduct pingProduct);
        Task<string> DeleteAsync(int id);
        Task<string> ToggleAsync(int id);
    }
}