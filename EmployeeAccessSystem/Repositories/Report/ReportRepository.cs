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

        public async Task<List<ReportModel>> GetReportDataAsync(string flag, int productId, DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection conn = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Flag", flag);
                parameters.Add("ProductId", productId);
                parameters.Add("FromDate", fromDate.Date);
                parameters.Add("ToDate", toDate.Date);

                IEnumerable<ReportModel> result = await conn.QueryAsync<ReportModel>(
                    "dbo.sp_Report_GetData",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }
    }
}