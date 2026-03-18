using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly ISupervisorRepositories _supervisorRepositories;

        public SupervisorService(ISupervisorRepositories supervisorRepositories)
        {
            _supervisorRepositories = supervisorRepositories;
        }

        public async Task<IEnumerable<AccessRequest>> GetRequestsForSupervisorAsync(int supervisorEmployeeId)
        {
            return await _supervisorRepositories.GetRequestsForSupervisorAsync(supervisorEmployeeId);
        }

        public async Task<string> ApproveRequestAsync(int requestId, int supervisorEmployeeId, string? comment)
        {
            var request = await _supervisorRepositories.GetRequestByIdAsync(requestId);

            if (request == null)
            {
                return "Request not found";
            }

            if (request.SupervisorStatus != "Pending")
            {
                return "This request has already been reviewed";
            }

            int result = await _supervisorRepositories.ApproveRequestAsync(requestId, supervisorEmployeeId, comment);

            if (result <= 0)
            {
                return "Request could not be approved";
            }

            return "";
        }

        public async Task<string> RejectRequestAsync(int requestId, int supervisorEmployeeId, string? comment)
        {
            var request = await _supervisorRepositories.GetRequestByIdAsync(requestId);

            if (request == null)
            {
                return "Request not found";
            }

            if (request.SupervisorStatus != "Pending")
            {
                return "This request has already been reviewed";
            }

            int result = await _supervisorRepositories.RejectRequestAsync(requestId, supervisorEmployeeId, comment);

            if (result <= 0)
            {
                return "Request could not be rejected";
            }

            return "";
        }
    }
}