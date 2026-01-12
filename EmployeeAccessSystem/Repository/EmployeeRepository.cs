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
            await conn.OpenAsync();

            var sql = "SELECT EmployeeId, FullName, Email, Department FROM Employees";
            var employees = await conn.QueryAsync<Employee>(sql);
            return employees;
        }


        public async Task<Employee> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var sql = "SELECT EmployeeId, FullName, Email, Department FROM Employees WHERE EmployeeId = @Id";
            var employee = await conn.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id });
            return employee;
        }

        public async Task<int> AddAsync(Employee employee)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var sql = "INSERT INTO Employees (FullName, Email, Department) VALUES (@FullName, @Email, @Department)";
            var result = await conn.ExecuteAsync(sql, employee);
            return result; 
        }


        public async Task<int> UpdateAsync(Employee employee)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var sql = "UPDATE Employees SET FullName=@FullName, Email=@Email, Department=@Department WHERE EmployeeId=@EmployeeId";
            var result = await conn.ExecuteAsync(sql, employee);
            return result;
        }


        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var sql = "DELETE FROM Employees WHERE EmployeeId=@Id";
            var result = await conn.ExecuteAsync(sql, new { Id = id });
            return result;
        }
    }
}
