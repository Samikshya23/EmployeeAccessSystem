using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetSupervisorsAsync();
        Task<string> UpdateAsync(Employee model);
        Task<string> ToggleAsync(int id);
        Task<string> DeleteAsync(int id);
    }
}