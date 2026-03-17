using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class AccessRequestService : IAccessRequestService
    {
        private readonly IAccessRequestRepositories _accessRequestRepository;

        public AccessRequestService(IAccessRequestRepositories accessRequestRepository)
        {
            _accessRequestRepository = accessRequestRepository;
        }

        public async Task<string> CreateRequestAsync(AccessRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AssetTag))
            {
                return "Please enter asset tag";
            }

            if (request.CategoryId <= 0)
            {
                return "Please select category";
            }

            if (request.SubCategoryId <= 0)
            {
                return "Please select server";
            }

            if (string.IsNullOrWhiteSpace(request.IPAddress))
            {
                return "Please enter IP address";
            }

            if (string.IsNullOrWhiteSpace(request.Duration))
            {
                return "Please select duration";
            }

            if (string.IsNullOrWhiteSpace(request.RequestReason))
            {
                return "Please enter justification";
            }

            request.AssetTag = request.AssetTag.Trim();
            request.IPAddress = request.IPAddress.Trim();
            request.Duration = request.Duration.Trim();
            request.RequestReason = request.RequestReason.Trim();

            int result = await _accessRequestRepository.CreateRequestAsync(request);

            if (result <= 0)
            {
                return "Request could not be saved";
            }

            return "";
        }

        public async Task<IEnumerable<AccessRequest>> GetEmployeeRequestsAsync(int employeeId)
        {
            return await _accessRequestRepository.GetEmployeeRequestsAsync(employeeId);
        }
    }
}