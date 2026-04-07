using System.Data;
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
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<ProductSetup>> GetAllAsync()
        {
            using var conn = GetConnection();

            return await conn.QueryAsync<ProductSetup>(
                "dbo.sp_ProductSetup_Manage",
                new { Flag = "GETALL" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ProductSetup> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("ProductId", id);

            return await conn.QueryFirstOrDefaultAsync<ProductSetup>("dbo.sp_ProductSetup_Manage", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddAsync(ProductSetup productSetup)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(                "dbo.sp_ProductSetup_Manage",                new
                {
                    Flag = "ADD",
                    productSetup.ProductName,
                    productSetup.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(ProductSetup productSetup)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_ProductSetup_Manage",
                new
                {
                    Flag = "UPDATE",
                    productSetup.ProductId,
                    productSetup.ProductName,
                    productSetup.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_ProductSetup_Manage",
                new
                {
                    Flag = "DELETE",
                    ProductId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ToggleAsync(int id)
        {
            using var conn = GetConnection();

            await conn.ExecuteAsync(
                "dbo.sp_ProductSetup_Manage",
                new
                {
                    Flag = "TOGGLE",
                    ProductId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductSetup>> GetActiveAsync()
        {
            using var conn = GetConnection();

            return await conn.QueryAsync<ProductSetup>(
                "dbo.sp_ProductSetup_Manage",
                new { Flag = "GETACTIVE" },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}