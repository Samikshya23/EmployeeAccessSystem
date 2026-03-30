using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IReportService
    {
        Task<ReportPageViewModel> GetReportPageAsync(int? selectedProductId, DateTime? fromDate, DateTime? toDate);
    }
}