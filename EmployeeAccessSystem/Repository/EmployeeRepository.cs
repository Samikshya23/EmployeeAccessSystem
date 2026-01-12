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

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

      
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var conn = GetConnection();
            var sql = "SELECT EmployeeId, FullName, Email, Department FROM Employees";
            return await conn.QueryAsync<Employee>(sql);
        }

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            using var conn = GetConnection();
            var sql = "SELECT * FROM Employees WHERE EmployeeId = @Id";
            return await conn.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = employeeId });
        }

       
        public async Task<Employee> GetByEmailAsync(string email)
        {
            using var conn = GetConnection();
            var sql = "SELECT * FROM Employees WHERE Email = @Email";
            return await conn.QueryFirstOrDefaultAsync<Employee>(sql, new { Email = email });
        }

        public async Task<int> AddAsync(Employee employee)
        {
            using var conn = GetConnection();

            var existing = await GetByEmailAsync(employee.Email);
            if (existing != null)
            {
                throw new System.Exception("Email already exists");
            }

            var sql = @"INSERT INTO Employees (FullName, Email, Department)
                        VALUES (@FullName, @Email, @Department)";

            return await conn.ExecuteAsync(sql, employee);
        }

        public async Task<int> UpdateAsync(Employee employee)
        {
            using var conn = GetConnection();

            var sql = @"UPDATE Employees 
                        SET FullName=@FullName, Email=@Email, Department=@Department
                        WHERE EmployeeId=@EmployeeId";

            return await conn.ExecuteAsync(sql, employee);
        }


        public async Task<int> DeleteAsync(int employeeId)
        {
            using var conn = GetConnection();
            var sql = "DELETE FROM Employees WHERE EmployeeId=@Id";
            return await conn.ExecuteAsync(sql, new { Id = employeeId });
        }
    }
}
