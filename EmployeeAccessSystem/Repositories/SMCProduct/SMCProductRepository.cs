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

        // 🔹 GET ALL
        public async Task<IEnumerable<SMCProduct>> GetAllAsync()
        {
            using var conn = GetConnection();

            return await conn.QueryAsync<SMCProduct>(
                "dbo.sp_SMCProduct_Manage",
                new { Flag = "GETALL" },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔹 GET ACTIVE
        public async Task<IEnumerable<SMCProduct>> GetActiveAsync()
        {
            using var conn = GetConnection();

            return await conn.QueryAsync<SMCProduct>(
                "dbo.sp_SMCProduct_Manage",
                new { Flag = "GETACTIVE" },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔹 GET BY PRODUCT (🔥 IMPORTANT FIX)
        public async Task<IEnumerable<SMCProduct>> GetByProductIdAsync(int productId)
        {
            using var conn = GetConnection();

            return await conn.QueryAsync<SMCProduct>(
                "dbo.sp_SMCProduct_Manage",
                new
                {
                    Flag = "GETBYPRODUCT",
                    ProductId = productId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔹 GET BY ID
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

        // 🔹 ADD (🔥 FIXED WITH ProductId)
        public async Task<int> AddAsync(SMCProduct smcProduct)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_SMCProduct_Manage",
                new
                {
                    Flag = "ADD",
                    ProductId = smcProduct.ProductId,
                    SMCProductName = smcProduct.SMCProductName,
                    IsActive = smcProduct.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔹 UPDATE (🔥 FIXED WITH ProductId)
        public async Task<int> UpdateAsync(SMCProduct smcProduct)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_SMCProduct_Manage",
                new
                {
                    Flag = "UPDATE",
                    SMCProductId = smcProduct.SMCProductId,
                    ProductId = smcProduct.ProductId,
                    SMCProductName = smcProduct.SMCProductName,
                    IsActive = smcProduct.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔹 DELETE
        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_SMCProduct_Manage",
                new
                {
                    Flag = "DELETE",
                    SMCProductId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // 🔹 TOGGLE
        public async Task ToggleAsync(int id)
        {
            using var conn = GetConnection();

            await conn.ExecuteAsync(
                "dbo.sp_SMCProduct_Manage",
                new
                {
                    Flag = "TOGGLE",
                    SMCProductId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}