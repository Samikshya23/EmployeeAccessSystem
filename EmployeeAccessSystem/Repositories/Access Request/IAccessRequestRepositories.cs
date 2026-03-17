using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IAccessRequestRepositories
    {
        Task<int> CreateRequestAsync(AccessRequest request);
        Task<IEnumerable<AccessRequest>> GetEmployeeRequestsAsync(int employeeId);
    }
}