using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<ProductSetup>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<ProductSetup>(
                "dbo.sp_ProductSetup_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductSetup>> GetActiveAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETACTIVE");

            return await conn.QueryAsync<ProductSetup>(
                "dbo.sp_ProductSetup_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ProductSetup> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("ProductId", id);

            return await conn.QueryFirstOrDefaultAsync<ProductSetup>(
                "dbo.sp_ProductSetup_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(ProductSetup productSetup)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("ProductName", productSetup.ProductName);
            parameters.Add("IsActive", productSetup.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_ProductSetup_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(ProductSetup productSetup)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("ProductId", productSetup.ProductId);
            parameters.Add("ProductName", productSetup.ProductName);
            parameters.Add("IsActive", productSetup.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_ProductSetup_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("ProductId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_ProductSetup_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("ProductId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_ProductSetup_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}