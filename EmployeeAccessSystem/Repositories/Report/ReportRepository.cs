using System.Data;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly string _connectionString;

        public ReportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<List<ReportModel>> GetReportDataAsync(int productId, DateTime fromDate, DateTime toDate)
        {
            using var conn = GetConnection();

            var result = await conn.QueryAsync<ReportModel>(
                "dbo.sp_Report_GetData",
                new
                {
                    ProductId = productId,
                    FromDate = fromDate.Date,
                    ToDate = toDate.Date
                },
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
    }
}