using System.Data;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class SMCConfigRepository : ISMCConfigRepository
    {
        private readonly string _connectionString;

        public SMCConfigRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<SMCConfig>> GetAllAsync()
        {
            using var conn = GetConnection();

            return await conn.QueryAsync<SMCConfig>(
                "dbo.sp_SMCConfig_Manage",
                new { Flag = "GETALL" },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SMCConfig?> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            return await conn.QueryFirstOrDefaultAsync<SMCConfig>(
                "dbo.sp_SMCConfig_Manage",
                new { Flag = "GETBYID", SMCConfigId = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SMCConfig?> GetExistingAsync(int productId, int smcProductId, int itemId, DateTime date)
        {
            using var conn = GetConnection();

            return await conn.QueryFirstOrDefaultAsync<SMCConfig>(
                "dbo.sp_SMCConfig_Manage",
                new
                {
                    Flag = "CHECKEXIST",
                    ProductId = productId,
                    SMCProductId = smcProductId,
                    SMCProductItemId = itemId,
                    EntryDate = date
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(SMCConfig model)
        {
            using var conn = GetConnection();

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCConfig_Manage",
                new
                {
                    Flag = "ADD",
                    ProductId = model.ProductId,
                    SMCProductId = model.SMCProductId,
                    SMCProductItemId = model.SMCProductItemId,
                    EntryDate = model.EntryDate,
                    ConfigValue = model.ConfigValue,
                    IsChecked = model.IsChecked,
                    EntryMode = model.EntryMode,
                    Remarks = model.Remarks,
                    IsActive = model.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(SMCConfig model)
        {
            using var conn = GetConnection();

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCConfig_Manage",
                new
                {
                    Flag = "UPDATE",
                    SMCConfigId = model.SMCConfigId,
                    ProductId = model.ProductId,
                    SMCProductId = model.SMCProductId,
                    SMCProductItemId = model.SMCProductItemId,
                    EntryDate = model.EntryDate,
                    ConfigValue = model.ConfigValue,
                    IsChecked = model.IsChecked,
                    EntryMode = model.EntryMode,
                    Remarks = model.Remarks,
                    IsActive = model.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCConfig_Manage",
                new { Flag = "DELETE", SMCConfigId = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCConfig_Manage",
                new { Flag = "TOGGLE", SMCConfigId = id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}