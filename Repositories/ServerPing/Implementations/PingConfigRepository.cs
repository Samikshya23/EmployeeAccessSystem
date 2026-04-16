using System.Data;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class PingConfigRepository : IPingConfigRepository
    {
        private readonly string _connectionString;

        public PingConfigRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<PingConfig>> GetAllAsync()
        {
            using (SqlConnection conn = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Flag", "GETALL");

                return await conn.QueryAsync<PingConfig>(
                    "dbo.sp_PingConfig_Manage",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<PingConfig?> GetByIdAsync(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Flag", "GETBYID");
                parameters.Add("PingConfigId", id);

                return await conn.QueryFirstOrDefaultAsync<PingConfig>(
                    "dbo.sp_PingConfig_Manage",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<PingConfig?> CheckDuplicateAsync(int pingProductId, DateTime entryDate)
        {
            using (SqlConnection conn = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Flag", "CHECKDUPLICATE");
                parameters.Add("PingProductId", pingProductId);
                parameters.Add("EntryDate", entryDate.Date);

                return await conn.QueryFirstOrDefaultAsync<PingConfig>(
                    "dbo.sp_PingConfig_Manage",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<PingConfig?> CheckDuplicateForUpdateAsync(int pingConfigId, int pingProductId, DateTime entryDate)
        {
            using (SqlConnection conn = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Flag", "CHECKDUPLICATEFORUPDATE");
                parameters.Add("PingConfigId", pingConfigId);
                parameters.Add("PingProductId", pingProductId);
                parameters.Add("EntryDate", entryDate.Date);

                return await conn.QueryFirstOrDefaultAsync<PingConfig>(
                    "dbo.sp_PingConfig_Manage",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> AddAsync(PingConfig pingConfig)
        {
            using (SqlConnection conn = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Flag", "ADD");
                parameters.Add("ProductId", pingConfig.ProductId);
                parameters.Add("PingProductId", pingConfig.PingProductId);
                parameters.Add("EntryDate", pingConfig.EntryDate.Date);
                parameters.Add("ConfigValue", pingConfig.ConfigValue);
                parameters.Add("Remarks", pingConfig.Remarks);
                parameters.Add("IsActive", pingConfig.IsActive);
                parameters.Add("IsChecked", pingConfig.IsChecked);
                parameters.Add("EntryMode", pingConfig.EntryMode);
                parameters.Add("CreatedBy", pingConfig.CreatedBy);

                return await conn.ExecuteScalarAsync<int>(
                    "dbo.sp_PingConfig_Manage",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> UpdateAsync(PingConfig pingConfig)
        {
            using (SqlConnection conn = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Flag", "UPDATE");
                parameters.Add("PingConfigId", pingConfig.PingConfigId);
                parameters.Add("ProductId", pingConfig.ProductId);
                parameters.Add("PingProductId", pingConfig.PingProductId);
                parameters.Add("EntryDate", pingConfig.EntryDate.Date);
                parameters.Add("ConfigValue", pingConfig.ConfigValue);
                parameters.Add("Remarks", pingConfig.Remarks);
                parameters.Add("IsActive", pingConfig.IsActive);
                parameters.Add("IsChecked", pingConfig.IsChecked);
                parameters.Add("EntryMode", pingConfig.EntryMode);
                parameters.Add("ModifiedBy", pingConfig.ModifiedBy);

                return await conn.ExecuteScalarAsync<int>(
                    "dbo.sp_PingConfig_Manage",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> DeleteAsync(int id, string deletedBy)
        {
            using (SqlConnection conn = GetConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Flag", "DELETE");
                parameters.Add("PingConfigId", id);
                parameters.Add("DeletedBy", deletedBy);

                return await conn.ExecuteScalarAsync<int>(
                    "dbo.sp_PingConfig_Manage",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}