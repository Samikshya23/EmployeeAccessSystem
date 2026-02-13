using EmployeeAccessSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);

        Task<string> AddAsync(Category category);
        Task<string> UpdateAsync(Category category);

        Task DeleteAsync(int id);
        Task ToggleAsync(int id);
    }
}
