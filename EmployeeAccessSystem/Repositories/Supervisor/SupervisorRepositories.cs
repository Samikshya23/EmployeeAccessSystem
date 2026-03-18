using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Repositories
{
    public class SupervisorRepositories : ISupervisorRepositories
    {
        private readonly string _connectionString;

        public SupervisorRepositories(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<AccessRequest>> GetRequestsForSupervisorAsync(int supervisorEmployeeId)
        {
            using var conn = GetConnection();

            string sql = @"
                SELECT
                    ar.RequestId,
                    ar.EmployeeId,
                    ar.CategoryId,
                    ar.SubCategoryId,
                    ar.AssetTag,
                    ar.Duration,
                    ar.RequestReason,
                    ar.RequestDate,
                    ar.SupervisorStatus,
                    ar.SupervisorComment,
                    ar.SupervisorActionByEmployeeId,
                    ar.SupervisorActionDate,
                    ar.FinalStatus,
                    e.FullName AS EmployeeName,
                    c.CategoryName,
                    s.ServerName,
                    s.ServerIP
                FROM AccessRequests ar
                INNER JOIN Employees e ON ar.EmployeeId = e.EmployeeId
                INNER JOIN Categories c ON ar.CategoryId = c.CategoryId
                INNER JOIN SubCategories s ON ar.SubCategoryId = s.SubCategoryId
                WHERE e.SupervisorEmployeeId = @SupervisorEmployeeId
                ORDER BY ar.RequestId DESC;";

            return await conn.QueryAsync<AccessRequest>(sql, new { SupervisorEmployeeId = supervisorEmployeeId });
        }

        public async Task<AccessRequest?> GetRequestByIdAsync(int requestId)
        {
            using var conn = GetConnection();

            string sql = @"
                SELECT
                    ar.RequestId,
                    ar.EmployeeId,
                    ar.CategoryId,
                    ar.SubCategoryId,
                    ar.AssetTag,
                    ar.Duration,
                    ar.RequestReason,
                    ar.RequestDate,
                    ar.SupervisorStatus,
                    ar.SupervisorComment,
                    ar.SupervisorActionByEmployeeId,
                    ar.SupervisorActionDate,
                    ar.FinalStatus,
                    e.FullName AS EmployeeName,
                    c.CategoryName,
                    s.ServerName,
                    s.ServerIP
                FROM AccessRequests ar
                INNER JOIN Employees e ON ar.EmployeeId = e.EmployeeId
                INNER JOIN Categories c ON ar.CategoryId = c.CategoryId
                INNER JOIN SubCategories s ON ar.SubCategoryId = s.SubCategoryId
                WHERE ar.RequestId = @RequestId;";

            return await conn.QueryFirstOrDefaultAsync<AccessRequest>(sql, new { RequestId = requestId });
        }

        public async Task<int> ApproveRequestAsync(int requestId, int supervisorEmployeeId, string? comment)
        {
            using var conn = GetConnection();

            string sql = @"
                UPDATE AccessRequests
                SET SupervisorStatus = 'Approved',
                    SupervisorComment = @SupervisorComment,
                    SupervisorActionByEmployeeId = @SupervisorEmployeeId,
                    SupervisorActionDate = GETDATE(),
                    FinalStatus = 'Approved'
                WHERE RequestId = @RequestId;";

            return await conn.ExecuteAsync(sql, new
            {
                RequestId = requestId,
                SupervisorEmployeeId = supervisorEmployeeId,
                SupervisorComment = comment
            });
        }

        public async Task<int> RejectRequestAsync(int requestId, int supervisorEmployeeId, string? comment)
        {
            using var conn = GetConnection();

            string sql = @"
                UPDATE AccessRequests
                SET SupervisorStatus = 'Rejected',
                    SupervisorComment = @SupervisorComment,
                    SupervisorActionByEmployeeId = @SupervisorEmployeeId,
                    SupervisorActionDate = GETDATE(),
                    FinalStatus = 'Rejected'
                WHERE RequestId = @RequestId;";

            return await conn.ExecuteAsync(sql, new
            {
                RequestId = requestId,
                SupervisorEmployeeId = supervisorEmployeeId,
                SupervisorComment = comment
            });
        }
    }
}