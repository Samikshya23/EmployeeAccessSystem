using EmployeeAccessSystem.Models;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public interface ISubCategoryService
    {
        Task CreateAsync(SubCategory model);
        Task UpdateAsync(SubCategory model);
        Task DeleteAsync(int id);
    }
}