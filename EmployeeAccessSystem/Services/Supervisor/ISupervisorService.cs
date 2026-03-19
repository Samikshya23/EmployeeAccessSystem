using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface ISupervisorService
    {
        Task<IEnumerable<AccessRequest>> GetPendingRequestsForSupervisorAsync(int supervisorEmployeeId, string? search);
        Task<IEnumerable<AccessRequest>> GetRequestHistoryForSupervisorAsync(int supervisorEmployeeId, string? search, string? status);
        Task<string> ApproveRequestAsync(int requestId, int supervisorEmployeeId, string? comment);
        Task<string> RejectRequestAsync(int requestId, int supervisorEmployeeId, string? comment);
    }
}