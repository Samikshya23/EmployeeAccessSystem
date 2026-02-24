using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var conn = GetConnection();
            var sql = "SELECT CategoryId, CategoryName, IsActive FROM dbo.Categories";
            return await conn.QueryAsync<Category>(sql);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            var sql = "SELECT CategoryId, CategoryName, IsActive FROM dbo.Categories WHERE CategoryId = @Id";
            return await conn.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Category category)
        {
            using var conn = GetConnection();
            var sql = "INSERT INTO dbo.Categories (CategoryName, IsActive) VALUES (@CategoryName, @IsActive)";
            return await conn.ExecuteAsync(sql, category);
        }

        public async Task<int> UpdateAsync(Category category)
        {
            using var conn = GetConnection();
            var sql = @"UPDATE dbo.Categories
                        SET CategoryName = @CategoryName,
                            IsActive = @IsActive
                        WHERE CategoryId = @CategoryId";
            return await conn.ExecuteAsync(sql, category);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();
            var sql = "DELETE FROM dbo.Categories WHERE CategoryId = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }

        public async Task ToggleAsync(int id)
        {
            using var conn = GetConnection();
            var sql = @"UPDATE dbo.Categories
                SET IsActive =
                    CASE 
                        WHEN IsActive = 1 THEN 0 
                        ELSE 1 
                    END
                WHERE CategoryId = @Id";

            await conn.ExecuteAsync(sql, new { Id = id });
        }
        public async Task<IEnumerable<Category>> GetActiveAsync()
        {
            using var conn = GetConnection();
            string sql = @"SELECT CategoryId, CategoryName
                   FROM Categories
                   WHERE IsActive = 1
                   ORDER BY CategoryName";
            return await conn.QueryAsync<Category>(sql);
        }


    }
}
