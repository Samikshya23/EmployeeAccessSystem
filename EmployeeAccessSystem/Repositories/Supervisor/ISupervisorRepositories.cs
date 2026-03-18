using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface ISupervisorRepositories
    {
        Task<IEnumerable<AccessRequest>> GetRequestsForSupervisorAsync(int supervisorEmployeeId);
        Task<AccessRequest?> GetRequestByIdAsync(int requestId);
        Task<int> ApproveRequestAsync(int requestId, int supervisorEmployeeId, string? comment);
        Task<int> RejectRequestAsync(int requestId, int supervisorEmployeeId, string? comment);
    }
}