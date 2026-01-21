using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
        {
            return new SqlConnection(_connectionString);
        }

        public List<Category> GetAll()
        {
            using var conn = GetConnection();
            string sql = "SELECT * FROM Categories ORDER BY CategoryId";
            return conn.Query<Category>(sql).ToList();
        }

        public void ToggleActive(int id)
        {
            using var conn = GetConnection();
            string sql = @"
UPDATE Categories
SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
WHERE CategoryId = @Id";
            conn.Execute(sql, new { Id = id });
        }
    }
}
