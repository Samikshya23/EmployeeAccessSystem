using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IAccessRequestService
    {
        Task<string> CreateRequestAsync(AccessRequest request);
        Task<IEnumerable<AccessRequest>> GetEmployeeRequestsAsync(int employeeId);
    }
}