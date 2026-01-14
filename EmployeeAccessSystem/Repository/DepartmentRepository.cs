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

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

     
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            using var conn = GetConnection();
            var sql = "SELECT DepartmentId, DepartmentName FROM Departments";
            return await conn.QueryAsync<Department>(sql);
        }

  
        public async Task<Department> GetByIdAsync(int departmentId)
        {
            using var conn = GetConnection();
            var sql = "SELECT DepartmentId, DepartmentName FROM Departments WHERE DepartmentId = @Id";
            return await conn.QueryFirstOrDefaultAsync<Department>(sql, new { Id = departmentId });
        }

    
        public async Task<int> AddAsync(Department department)
        {
            using var conn = GetConnection();
            var sql = "INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName)";
            return await conn.ExecuteAsync(sql, department);
        }


        public async Task<int> UpdateAsync(Department department)
        {
            using var conn = GetConnection();
            var sql = "UPDATE Departments SET DepartmentName=@DepartmentName WHERE DepartmentId=@DepartmentId";
            return await conn.ExecuteAsync(sql, department);
        }

   
        public async Task<int> DeleteAsync(int departmentId)
        {
            using var conn = GetConnection();
            var sql = "DELETE FROM Departments WHERE DepartmentId=@Id";
            return await conn.ExecuteAsync(sql, new { Id = departmentId });
        }
    }
}
