using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Repositories
{
    public class AccessRequestRepositories : IAccessRequestRepositories
    {
        private readonly string _connectionString;

        public AccessRequestRepositories(IConfiguration configuration)
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
                    AssetTag,
                    IPAddress,
                    Duration,
                    RequestReason
                )
                VALUES
                (
                    @EmployeeId,
                    @CategoryId,
                    @SubCategoryId,
                    @AssetTag,
                    @IPAddress,
                    @Duration,
                    @RequestReason
                )";

            return await conn.ExecuteAsync(sql, request);
        }

        public async Task<IEnumerable<AccessRequest>> GetEmployeeRequestsAsync(int employeeId)
        {
            using var conn = GetConnection();

            string sql = @"
                SELECT
                    ar.RequestId,
                    ar.EmployeeId,
                    ar.CategoryId,
                    ar.SubCategoryId,
                    ar.AssetTag,
                    ar.IPAddress,
                    ar.Duration,
                    ar.RequestReason,
                    ar.RequestDate,
                    ar.SupervisorStatus,
                    ar.SupervisorComment,
                    ar.SupervisorActionByEmployeeId,
                    ar.SupervisorActionDate,
                    ar.AdminStatus,
                    ar.AdminComment,
                    ar.AdminActionByEmployeeId,
                    ar.AdminActionDate,
                    ar.FinalStatus,
                    c.CategoryName,
                    s.ServerName
                FROM AccessRequests ar
                INNER JOIN Categories c ON ar.CategoryId = c.CategoryId
                INNER JOIN SubCategories s ON ar.SubCategoryId = s.SubCategoryId
                WHERE ar.EmployeeId = @EmployeeId
                ORDER BY ar.RequestId DESC";

            return await conn.QueryAsync<AccessRequest>(sql, new { EmployeeId = employeeId });
        }
    }
}