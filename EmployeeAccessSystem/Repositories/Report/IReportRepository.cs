using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IReportRepository
    {
        Task<List<ReportModel>> GetReportDataAsync(int productId, DateTime fromDate, DateTime toDate);
    }
}