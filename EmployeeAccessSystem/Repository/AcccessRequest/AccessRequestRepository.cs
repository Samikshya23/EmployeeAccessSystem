using Dapper;
using Microsoft.Data.SqlClient;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public class AccessRequestRepository : IAccessRequestRepository
    {
        private readonly string _connectionString;

        public AccessRequestRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<int> CreateRequestAsync(AccessRequest request)
        {
            using var conn = GetConnection();

            string sql = @"
                INSERT INTO AccessRequests
                (
                    EmployeeId,
                    CategoryId,
                    SubCategoryId,
                    RequestReason
                )
                VALUES
                (
                    @EmployeeId,
                    @CategoryId,
                    @SubCategoryId,
                    @RequestReason
                );";

            return await conn.ExecuteAsync(sql, request);
        }

        public async Task<IEnumerable<AccessRequest>> GetEmployeeRequestsAsync(int employeeId)
        {
            using var conn = GetConnection();

            string sql = @"
                SELECT *
                FROM AccessRequests
                WHERE EmployeeId = @EmployeeId
                ORDER BY RequestId DESC;";

            return await conn.QueryAsync<AccessRequest>(sql, new { EmployeeId = employeeId });
        }
    }
}