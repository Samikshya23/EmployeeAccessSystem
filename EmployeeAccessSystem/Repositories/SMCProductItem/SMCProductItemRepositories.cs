using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
                new
                {
                    Flag = "GETALL"
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<SMCProductItem>> GetActiveAsync()
        {
            using var conn = GetConnection();

            return await conn.QueryAsync<SMCProductItem>(
                "dbo.sp_SMCProductItem_Manage",
                new
                {
                    Flag = "GETACTIVE"
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

        public async Task<SMCProductItem> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("SMCProductItemId", id);

            return await conn.QueryFirstOrDefaultAsync<SMCProductItem>(
                "dbo.sp_SMCProductItem_Manage",
                parameters,
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
                    SMCProductId = model.SMCProductId,
                    ItemName = model.ItemName,
                    //ValueType = model.ValueType,
                    IsActive = model.IsActive
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
                    SMCProductItemId = model.SMCProductItemId,
                    SMCProductId = model.SMCProductId,
                    ItemName = model.ItemName,
                    //ValueType = model.ValueType,
                    IsActive = model.IsActive
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
    }
}