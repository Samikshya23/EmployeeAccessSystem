using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class AccessRequestService : IAccessRequestService
    {
        private readonly IAccessRequestRepository _accessRequestRepository;

        public AccessRequestService(IAccessRequestRepository accessRequestRepository)
        {
            _accessRequestRepository = accessRequestRepository;
        }

        public async Task<string> CreateRequestAsync(AccessRequest request)
        {
            if (request.CategoryId <= 0)
            {
                return "Please select category";
            }

            if (request.SubCategoryId <= 0)
            {
                return "Please select sub category";
            }

            if (string.IsNullOrWhiteSpace(request.RequestReason))
            {
                return "Please enter request reason";
            }

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