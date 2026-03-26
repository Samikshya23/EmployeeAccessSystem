using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public class SMCProductItemRepositories : ISMCProductItemRepositories
    {
        private readonly string _connectionString;

        public SMCProductItemRepositories(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<SMCProductItem>> GetAllAsync()
        {
            using var conn = GetConnection();

            return await conn.QueryAsync<SMCProductItem>(
                "dbo.sp_SMCProductItem_Manage",
                new { Flag = "GETALL" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SMCProductItem> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            return await conn.QueryFirstOrDefaultAsync<SMCProductItem>(
                "dbo.sp_SMCProductItem_Manage",
                new
                {
                    Flag = "GETBYID",
                    SMCProductItemId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(SMCProductItem model)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_SMCProductItem_Manage",
                new
                {
                    Flag = "ADD",
                    model.SMCProductId,
                    model.ItemName
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(SMCProductItem model)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_SMCProductItem_Manage",
                new
                {
                    Flag = "UPDATE",
                    model.SMCProductItemId,
                    model.SMCProductId,
                    model.ItemName,
                    model.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_SMCProductItem_Manage",
                new
                {
                    Flag = "DELETE",
                    SMCProductItemId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ToggleAsync(int id)
        {
            using var conn = GetConnection();

            await conn.ExecuteAsync(
                "dbo.sp_SMCProductItem_Manage",
                new
                {
                    Flag = "TOGGLE",
                    SMCProductItemId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<SMCProductItem>> GetByProductAsync(int smcProductId)
        {
            using var conn = GetConnection();

            return await conn.QueryAsync<SMCProductItem>(
                "dbo.sp_SMCProductItem_Manage",
                new
                {
                    Flag = "GETBYPRODUCT",
                    SMCProductId = smcProductId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}