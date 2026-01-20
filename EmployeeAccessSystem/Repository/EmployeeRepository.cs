using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
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
SELECT e.EmployeeId, e.FullName, e.Email, e.DepartmentId,
       d.DepartmentName,
       e.CategoryId,
       c.CategoryName
FROM dbo.Employees e
INNER JOIN dbo.Departments d ON e.DepartmentId = d.DepartmentId
LEFT JOIN dbo.Category c ON e.CategoryId = c.CategoryId
ORDER BY e.EmployeeId DESC;";

            return await conn.QueryAsync<Employee>(sql);
        }

    
        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            using var conn = GetConnection();

            var sql = @"
SELECT e.EmployeeId, e.FullName, e.Email, e.DepartmentId,
       d.DepartmentName,
       e.CategoryId,
       c.CategoryName
FROM dbo.Employees e
INNER JOIN dbo.Departments d ON e.DepartmentId = d.DepartmentId
LEFT JOIN dbo.Category c ON e.CategoryId = c.CategoryId
WHERE e.EmployeeId = @Id;";

            return await conn.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = employeeId });
        }

      
        public async Task<Employee> GetByEmailAsync(string email)
        {
            using var conn = GetConnection();

            var sql = @"
SELECT e.EmployeeId, e.FullName, e.Email, e.DepartmentId,
       d.DepartmentName,
       e.CategoryId,
       c.CategoryName
FROM dbo.Employees e
INNER JOIN dbo.Departments d ON e.DepartmentId = d.DepartmentId
LEFT JOIN dbo.Category c ON e.CategoryId = c.CategoryId
WHERE e.Email = @Email;";

            return await conn.QueryFirstOrDefaultAsync<Employee>(sql, new { Email = email });
        }

        public async Task<int> AddAsync(Employee employee)
        {
            using var conn = GetConnection();

            var sql = @"
INSERT INTO dbo.Employees (FullName, Email, DepartmentId, CategoryId)
VALUES (@FullName, @Email, @DepartmentId, @CategoryId);";

            return await conn.ExecuteAsync(sql, employee);
        }

        public async Task<int> UpdateAsync(Employee employee)
        {
            using var conn = GetConnection();

            var sql = @"
UPDATE dbo.Employees
SET FullName = @FullName,
    Email = @Email,
    DepartmentId = @DepartmentId,
    CategoryId = @CategoryId
WHERE EmployeeId = @EmployeeId;";

            return await conn.ExecuteAsync(sql, employee);
        }


        public async Task<int> DeleteAsync(int employeeId)
        {
            using var conn = GetConnection();

            var sql = "DELETE FROM dbo.Employees WHERE EmployeeId = @Id";

            return await conn.ExecuteAsync(sql, new { Id = employeeId });
        }
    }
}
