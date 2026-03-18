using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface ISupervisorService
    {
        Task<IEnumerable<AccessRequest>> GetRequestsForSupervisorAsync(int supervisorEmployeeId);
        Task<string> ApproveRequestAsync(int requestId, int supervisorEmployeeId, string? comment);
        Task<string> RejectRequestAsync(int requestId, int supervisorEmployeeId, string? comment);
    }
}