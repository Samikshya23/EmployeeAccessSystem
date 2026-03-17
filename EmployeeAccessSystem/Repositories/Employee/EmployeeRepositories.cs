using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public class EmployeeRepositories : IEmployeeRepositories
    {
        private readonly string _connectionString;

        public EmployeeRepositories(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var conn = GetConnection();

            var sql = @"
                SELECT e.EmployeeId,
                       e.FullName,
                       e.Email,
                       e.DepartmentId,
                       e.AccountId,
                       d.DepartmentName,
                       e.IsActive,
                       e.SupervisorEmployeeId,
                       ISNULL(s.FullName, '') AS SupervisorName,
                       ISNULL(r.RoleName, '') AS Role
                FROM dbo.Employees e
                INNER JOIN dbo.Departments d ON e.DepartmentId = d.DepartmentId
                LEFT JOIN dbo.Employees s ON e.SupervisorEmployeeId = s.EmployeeId
                LEFT JOIN dbo.UserRoles ur ON e.AccountId = ur.AccountId
                LEFT JOIN dbo.Roles r ON ur.RoleId = r.RoleId
                ORDER BY e.EmployeeId DESC;";

            return await conn.QueryAsync<Employee>(sql);
        }

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            using var conn = GetConnection();

            var sql = @"
                SELECT e.EmployeeId,
                       e.FullName,
                       e.Email,
                       e.DepartmentId,
                       e.AccountId,
                       d.DepartmentName,
                       e.IsActive,
                       e.SupervisorEmployeeId,
                       ISNULL(s.FullName, '') AS SupervisorName,
                       ISNULL(r.RoleName, '') AS Role
                FROM dbo.Employees e
                INNER JOIN dbo.Departments d ON e.DepartmentId = d.DepartmentId
                LEFT JOIN dbo.Employees s ON e.SupervisorEmployeeId = s.EmployeeId
                LEFT JOIN dbo.UserRoles ur ON e.AccountId = ur.AccountId
                LEFT JOIN dbo.Roles r ON ur.RoleId = r.RoleId
                WHERE e.EmployeeId = @Id;";

            return await conn.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = employeeId });
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            using var conn = GetConnection();

            var sql = @"
                SELECT e.EmployeeId,
                       e.FullName,
                       e.Email,
                       e.DepartmentId,
                       e.AccountId,
                       d.DepartmentName,
                       e.IsActive,
                       e.SupervisorEmployeeId,
                       ISNULL(s.FullName, '') AS SupervisorName,
                       ISNULL(r.RoleName, '') AS Role
                FROM dbo.Employees e
                INNER JOIN dbo.Departments d ON e.DepartmentId = d.DepartmentId
                LEFT JOIN dbo.Employees s ON e.SupervisorEmployeeId = s.EmployeeId
                LEFT JOIN dbo.UserRoles ur ON e.AccountId = ur.AccountId
                LEFT JOIN dbo.Roles r ON ur.RoleId = r.RoleId
                WHERE e.Email = @Email;";

            return await conn.QueryFirstOrDefaultAsync<Employee>(sql, new { Email = email });
        }

        public async Task<IEnumerable<Employee>> GetSupervisorsAsync()
        {
            using var conn = GetConnection();

            var sql = @"
                SELECT e.EmployeeId,
                       e.FullName
                FROM dbo.Employees e
                INNER JOIN dbo.UserRoles ur ON e.AccountId = ur.AccountId
                INNER JOIN dbo.Roles r ON ur.RoleId = r.RoleId
                WHERE r.RoleName = 'Supervisor'
                ORDER BY e.FullName;";

            return await conn.QueryAsync<Employee>(sql);
        }

        public async Task<int> AddAsync(Employee employee)
        {
            using var conn = GetConnection();

            var sql = @"
                INSERT INTO dbo.Employees
                (
                    FullName,
                    Email,
                    DepartmentId,
                    AccountId,
                    IsActive,
                    SupervisorEmployeeId
                )
                VALUES
                (
                    @FullName,
                    @Email,
                    @DepartmentId,
                    @AccountId,
                    @IsActive,
                    @SupervisorEmployeeId
                );";

            try
            {
                return await conn.ExecuteAsync(sql, employee);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                return -1;
            }
        }

        public async Task<int> UpdateAsync(Employee employee)
        {
            using var conn = GetConnection();

            var sql = @"
                UPDATE dbo.Employees
                SET FullName = @FullName,
                    Email = @Email,
                    DepartmentId = @DepartmentId,
                    IsActive = @IsActive,
                    SupervisorEmployeeId = @SupervisorEmployeeId
                WHERE EmployeeId = @EmployeeId;";

            return await conn.ExecuteAsync(sql, employee);
        }

        public async Task<int> DeleteAsync(int employeeId)
        {
            using var conn = GetConnection();

            var clearSql = @"
                UPDATE dbo.Employees
                SET SupervisorEmployeeId = NULL
                WHERE SupervisorEmployeeId = @Id;";

            await conn.ExecuteAsync(clearSql, new { Id = employeeId });

            var sql = "DELETE FROM dbo.Employees WHERE EmployeeId = @Id;";
            return await conn.ExecuteAsync(sql, new { Id = employeeId });
        }

        public async Task ToggleAsync(int employeeId)
        {
            using var conn = GetConnection();

            var sql = @"
                UPDATE dbo.Employees
                SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                WHERE EmployeeId = @Id;";

            await conn.ExecuteAsync(sql, new { Id = employeeId });
        }
    }
}