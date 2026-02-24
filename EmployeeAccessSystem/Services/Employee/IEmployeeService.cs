

using EmployeeAccessSystem.Models;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public interface IEmployeeService
    {
        Task<Employee> GetByIdAsync(int id);
        Task<string> UpdateAsync(Employee model);
        Task<string> ToggleAsync(int id);
        Task<string> DeleteAsync(int id);
    }
}