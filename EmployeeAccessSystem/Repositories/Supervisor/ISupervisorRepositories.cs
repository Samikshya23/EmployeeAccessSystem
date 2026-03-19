using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface ISupervisorRepositories
    {
        Task<IEnumerable<AccessRequest>> GetPendingRequestsForSupervisorAsync(int supervisorEmployeeId, string? search);
        Task<IEnumerable<AccessRequest>> GetRequestHistoryForSupervisorAsync(int supervisorEmployeeId, string? search, string? status);
        Task<AccessRequest?> GetRequestByIdAsync(int requestId);
        Task<int> ApproveRequestAsync(int requestId, int supervisorEmployeeId, string? comment);
        Task<int> RejectRequestAsync(int requestId, int supervisorEmployeeId, string? comment);
    }
}