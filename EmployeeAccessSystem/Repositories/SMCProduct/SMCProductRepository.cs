using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Repositories
{
    public class SMCProductRepository : ISMCProductRepository
    {
        private readonly string _connectionString;
        public SMCProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public async Task<IEnumerable<SMCProduct>> GetAllAsync()
        {
            using var connection = GetConnection();
            string sql = "SELECT SMCProductId, SMCProductName FROM SMCProducts ORDER BY SMCProductId DESC";
            return await connection.QueryAsync<SMCProduct>(sql);
        }
        public async Task<SMCProduct> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            string sql = "SELECT SMCProductId, SMCProductName FROM SMCProducts WHERE SMCProductId = @SMCProductId";
            return await connection.QueryFirstOrDefaultAsync<SMCProduct>(sql, new { SMCProductId = id });
        }
        public async Task AddAsync(SMCProduct smcProduct)
        {
            using var connection = GetConnection();
            string sql = "INSERT INTO SMCProducts (SMCProductName) VALUES (@SMCProductName)";
            await connection.ExecuteAsync(sql, smcProduct);
        }
        public async Task UpdateAsync(SMCProduct smcProduct)
        {
            using var connection = GetConnection();
            string sql = @"UPDATE SMCProducts
                           SET SMCProductName = @SMCProductName
                           WHERE SMCProductId = @SMCProductId";
            await connection.ExecuteAsync(sql, smcProduct);
        }
        public async Task DeleteAsync(int id)
        {
            using var connection = GetConnection();
            string sql = "DELETE FROM SMCProducts WHERE SMCProductId = @SMCProductId";
            await connection.ExecuteAsync(sql, new { SMCProductId = id });
        }
    }
}
