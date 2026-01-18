using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;

        public DepartmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            using var conn = GetConnection();
            var sql = "SELECT DepartmentId, DepartmentName FROM dbo.Departments";
            return await conn.QueryAsync<Department>(sql);
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            var sql = "SELECT DepartmentId, DepartmentName FROM dbo.Departments WHERE DepartmentId = @Id";
            return await conn.QueryFirstOrDefaultAsync<Department>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Department department)
        {
            using var conn = GetConnection();
            var sql = "INSERT INTO dbo.Departments (DepartmentName) VALUES (@DepartmentName)";
            return await conn.ExecuteAsync(sql, department);
        }

        public async Task<int> UpdateAsync(Department department)
        {
            using var conn = GetConnection();
            var sql = @"UPDATE dbo.Departments
                        SET DepartmentName = @DepartmentName
                        WHERE DepartmentId = @DepartmentId";
            return await conn.ExecuteAsync(sql, department);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();
            var sql = "DELETE FROM dbo.Departments WHERE DepartmentId = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
