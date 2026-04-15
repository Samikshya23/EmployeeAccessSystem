using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class PingProductRepository : IPingProductRepository
    {
        private readonly string _connectionString;

        public PingProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<PingProduct>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<PingProduct>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<PingProduct>> GetActiveAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETACTIVE");

            return await conn.QueryAsync<PingProduct>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<PingProduct>> GetByProductIdAsync(int productId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYPRODUCT");
            parameters.Add("ProductId", productId);

            return await conn.QueryAsync<PingProduct>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProduct> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("PingProductId", id);

            return await conn.QueryFirstOrDefaultAsync<PingProduct>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProduct> CheckDuplicateAsync(int productId, string pingProductName)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKDUPLICATE");
            parameters.Add("ProductId", productId);
            parameters.Add("PingProductName", pingProductName);

            return await conn.QueryFirstOrDefaultAsync<PingProduct>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProduct> CheckDuplicateForUpdateAsync(int pingProductId, int productId, string pingProductName)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKDUPLICATEFORUPDATE");
            parameters.Add("PingProductId", pingProductId);
            parameters.Add("ProductId", productId);
            parameters.Add("PingProductName", pingProductName);

            return await conn.QueryFirstOrDefaultAsync<PingProduct>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(PingProduct pingProduct)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("ProductId", pingProduct.ProductId);
            parameters.Add("PingProductName", pingProduct.PingProductName);
            parameters.Add("IsActive", pingProduct.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(PingProduct pingProduct)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("PingProductId", pingProduct.PingProductId);
            parameters.Add("ProductId", pingProduct.ProductId);
            parameters.Add("PingProductName", pingProduct.PingProductName);
            parameters.Add("IsActive", pingProduct.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("PingProductId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("PingProductId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}