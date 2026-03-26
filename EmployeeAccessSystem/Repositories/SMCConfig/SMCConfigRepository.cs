using System.Data;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;

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

        public async Task<SMCConfig> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            return await conn.QueryFirstOrDefaultAsync<SMCConfig>(
                "dbo.sp_SMCConfig_Manage",
                new
                {
                    Flag = "GETBYID",
                    SMCConfigId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(SMCConfig model)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_SMCConfig_Manage",
                new
                {
                    Flag = "ADD",
                    model.ProductId,
                    model.SMCProductId,
                    model.SMCProductItemId,
                    model.EntryDate,
                    model.ConfigValue,
                    model.Remarks
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(SMCConfig model)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_SMCConfig_Manage",
                new
                {
                    Flag = "UPDATE",
                    model.SMCConfigId,
                    model.ProductId,
                    model.SMCProductId,
                    model.SMCProductItemId,
                    model.EntryDate,
                    model.ConfigValue,
                    model.Remarks,
                    model.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            return await conn.ExecuteAsync(
                "dbo.sp_SMCConfig_Manage",
                new
                {
                    Flag = "DELETE",
                    SMCConfigId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ToggleAsync(int id)
        {
            using var conn = GetConnection();

            await conn.ExecuteAsync(
                "dbo.sp_SMCConfig_Manage",
                new
                {
                    Flag = "TOGGLE",
                    SMCConfigId = id
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}