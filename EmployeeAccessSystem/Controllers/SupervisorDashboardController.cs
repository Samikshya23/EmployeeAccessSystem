using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeAccessSystem.Controllers
{
    public class SupervisorDashboardController : Controller
    {
        private readonly ISupervisorService _supervisorService;
        private readonly IEmployeeRepositories _employeeRepositories;

        public SupervisorDashboardController(
            ISupervisorService supervisorService,
            IEmployeeRepositories employeeRepositories)
        {
            _supervisorService = supervisorService;
            _employeeRepositories = employeeRepositories;
        }

        public IActionResult Index()
        {
            return RedirectToAction("PendingRequests");
        }

        public async Task<IActionResult> PendingRequests()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var supervisor = await _employeeRepositories.GetByEmailAsync(email);

            if (supervisor == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var requests = await _supervisorService.GetRequestsForSupervisorAsync(supervisor.EmployeeId)
                          ?? new List<AccessRequest>();

            return View("~/Views/SupervisorDashboard/PendingRequests.cshtml", requests);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int requestId, string? comment)
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var supervisor = await _employeeRepositories.GetByEmailAsync(email);

            if (supervisor == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string error = await _supervisorService.ApproveRequestAsync(requestId, supervisor.EmployeeId, comment);

            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return RedirectToAction("PendingRequests");
            }

            TempData["Success"] = "Request approved successfully.";
            return RedirectToAction("PendingRequests");
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int requestId, string? comment)
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var supervisor = await _employeeRepositories.GetByEmailAsync(email);

            if (supervisor == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string error = await _supervisorService.RejectRequestAsync(requestId, supervisor.EmployeeId, comment);

            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return RedirectToAction("PendingRequests");
            }

            TempData["Success"] = "Request rejected successfully.";
            return RedirectToAction("PendingRequests");
        }
    }
}