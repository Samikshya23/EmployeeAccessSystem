using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<SMCProduct>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<SMCProduct>> GetActiveAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETACTIVE");

            return await conn.QueryAsync<SMCProduct>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<SMCProduct>> GetByProductIdAsync(int productId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYPRODUCT");
            parameters.Add("ProductId", productId);

            return await conn.QueryAsync<SMCProduct>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SMCProduct> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("SMCProductId", id);

            return await conn.QueryFirstOrDefaultAsync<SMCProduct>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SMCProduct> CheckDuplicateAsync(int productId, string smcProductName)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKDUPLICATE");
            parameters.Add("ProductId", productId);
            parameters.Add("SMCProductName", smcProductName);

            return await conn.QueryFirstOrDefaultAsync<SMCProduct>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SMCProduct> CheckDuplicateForUpdateAsync(int smcProductId, int productId, string smcProductName)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKDUPLICATEFORUPDATE");
            parameters.Add("SMCProductId", smcProductId);
            parameters.Add("ProductId", productId);
            parameters.Add("SMCProductName", smcProductName);

            return await conn.QueryFirstOrDefaultAsync<SMCProduct>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(SMCProduct smcProduct)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("ProductId", smcProduct.ProductId);
            parameters.Add("SMCProductName", smcProduct.SMCProductName);
            parameters.Add("IsActive", smcProduct.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(SMCProduct smcProduct)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("SMCProductId", smcProduct.SMCProductId);
            parameters.Add("ProductId", smcProduct.ProductId);
            parameters.Add("SMCProductName", smcProduct.SMCProductName);
            parameters.Add("IsActive", smcProduct.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("SMCProductId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("SMCProductId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCProduct_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}