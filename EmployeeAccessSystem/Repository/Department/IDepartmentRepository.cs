using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();       
        Task<Department> GetByIdAsync(int departmentId);    
        Task<int> AddAsync(Department department);         
        Task<int> UpdateAsync(Department department);       
        Task<int> DeleteAsync(int departmentId);          
    }
}
