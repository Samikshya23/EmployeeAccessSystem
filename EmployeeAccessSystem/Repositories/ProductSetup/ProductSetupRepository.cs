using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public class ProductSetupRepositories : IProductSetupRepositories
    {
        private readonly string _connectionString;

        public ProductSetupRepositories(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_connectionString);

        public async Task<IEnumerable<ProductSetup>> GetAllAsync()
        {
            using var conn = GetConnection();
            var sql = "SELECT ProductId, ProductName, IsActive FROM dbo.ProductSetup";
            return await conn.QueryAsync<ProductSetup>(sql);
        }

        public async Task<ProductSetup> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            var sql = "SELECT ProductId, ProductName, IsActive FROM dbo.ProductSetup WHERE ProductId = @Id";
            return await conn.QueryFirstOrDefaultAsync<ProductSetup>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(ProductSetup productSetup)
        {
            using var conn = GetConnection();
            var sql = "INSERT INTO dbo.ProductSetup (ProductName, IsActive) VALUES (@ProductName, @IsActive)";
            return await conn.ExecuteAsync(sql, productSetup);
        }

        public async Task<int> UpdateAsync(ProductSetup productSetup)
        {
            using var conn = GetConnection();
            var sql = @"UPDATE dbo.ProductSetup
                        SET ProductName = @ProductName,
                            IsActive = @IsActive
                        WHERE ProductId = @ProductId";
            return await conn.ExecuteAsync(sql, productSetup);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();
            var sql = "DELETE FROM dbo.ProductSetup WHERE ProductId = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }

        public async Task ToggleAsync(int id)
        {
            using var conn = GetConnection();
            var sql = @"UPDATE dbo.ProductSetup
                        SET IsActive =
                            CASE 
                                WHEN IsActive = 1 THEN 0
                                ELSE 1
                            END
                        WHERE ProductId = @Id";

            await conn.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<ProductSetup>> GetActiveAsync()
        {
            using var conn = GetConnection();
            string sql = @"SELECT ProductId, ProductName
                           FROM dbo.ProductSetup
                           WHERE IsActive = 1
                           ORDER BY ProductName";
            return await conn.QueryAsync<ProductSetup>(sql);
        }
    }
}