using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();        // Get all departments
        Task<Department> GetByIdAsync(int departmentId);    // Get one department by ID
        Task<int> AddAsync(Department department);          // Add new department
        Task<int> UpdateAsync(Department department);       // Update department
        Task<int> DeleteAsync(int departmentId);            // Delete department
    }
}
