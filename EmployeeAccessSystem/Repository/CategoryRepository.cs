using Dapper;
using EmployeeAccessSystem.Models;

using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ICoreDbConnection _db;

        public CategoryRepository(ICoreDbConnection db)
        {
            _db = db;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_db.ConnectionString);
        }

      
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var sql = "SELECT CategoryId, CategoryName FROM Category";
            using var connection = GetConnection();
            return await connection.QueryAsync<Category>(sql);
        }

        
        public async Task<Category?> GetByIdAsync(int id)
        {
            var sql = "SELECT CategoryId, CategoryName FROM Category WHERE CategoryId = @Id";
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
        }

 
        public async Task<int> AddAsync(Category category)
        {
            var sql = "INSERT INTO Category (CategoryName) VALUES (@CategoryName)";
            using var connection = GetConnection();
            return await connection.ExecuteAsync(sql, category);
        }

   
        public async Task<int> UpdateAsync(Category category)
        {
            var sql = "UPDATE Category SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId";
            using var connection = GetConnection();
            return await connection.ExecuteAsync(sql, category);
        }

      
        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Category WHERE CategoryId = @Id";
            using var connection = GetConnection();
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
