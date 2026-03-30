using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAccessSystem.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> Index(int? selectedProductId, DateTime? fromDate, DateTime? toDate)
        {
            var model = await _reportService.GetReportPageAsync(selectedProductId, fromDate, toDate);
            return View(model);
        }
    }
}