using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly string _connectionString;

        public SubCategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_connectionString);
        public async Task<IEnumerable<SubCategory>> GetAllAsync()
        {
            using var conn = GetConnection();
            var sql = @"
                SELECT 
                    s.SubCategoryId,
                    s.CategoryId,
                    s.ServerIP,
                    s.ServerName,
                    c.CategoryName
                FROM dbo.SubCategories s
                INNER JOIN dbo.Categories c 
                ON c.CategoryId = s.CategoryId
                ORDER BY s.SubCategoryId DESC";
            return await conn.QueryAsync<SubCategory>(sql);
        }

        public async Task<SubCategory> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            var sql = @"SELECT SubCategoryId, CategoryId, ServerIP, ServerName
                        FROM dbo.SubCategories
                        WHERE SubCategoryId = @Id";
            return await conn.QueryFirstOrDefaultAsync<SubCategory>(sql, new { Id = id });
        }

       
        public async Task<int> AddAsync(SubCategory subCategory)
        {
            using var conn = GetConnection();
            var sql = @"INSERT INTO dbo.SubCategories (CategoryId, ServerIP, ServerName)
                        VALUES (@CategoryId, @ServerIP, @ServerName)";
            return await conn.ExecuteAsync(sql, subCategory);
        }

        
        public async Task<int> UpdateAsync(SubCategory subCategory)
        {
            using var conn = GetConnection();
            var sql = @"UPDATE dbo.SubCategories
                        SET CategoryId = @CategoryId,
                            ServerIP   = @ServerIP,
                            ServerName = @ServerName
                        WHERE SubCategoryId = @SubCategoryId";
            return await conn.ExecuteAsync(sql, subCategory);
        }


        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();
            var sql = "DELETE FROM dbo.SubCategories WHERE SubCategoryId = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
