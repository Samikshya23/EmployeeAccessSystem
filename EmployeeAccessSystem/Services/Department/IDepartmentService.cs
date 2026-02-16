using EmployeeAccessSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(int id);
    }
}