using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface ISubCategoryRepository
    {
        Task<IEnumerable<SubCategory>> GetAllAsync();
        Task<SubCategory> GetByIdAsync(int id);
        Task<int> AddAsync(SubCategory subCategory);
        Task<int> UpdateAsync(SubCategory subCategory);
        Task<int> DeleteAsync(int id);
    }
}
