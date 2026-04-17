using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IReportRepository
    {
        Task<List<ReportModel>> GetReportDataAsync(string flag, int productId, DateTime fromDate, DateTime toDate);
    }
}