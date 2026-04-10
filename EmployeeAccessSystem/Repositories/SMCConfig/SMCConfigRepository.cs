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
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<SMCConfig>(
                "dbo.sp_SMCConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SMCConfig?> GetByIdAsync(int id)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("SMCConfigId", id);

            return await conn.QueryFirstOrDefaultAsync<SMCConfig>(
                "dbo.sp_SMCConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SMCConfig?> GetExistingAsync(int productId, int smcProductId, int itemId, DateTime date)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKEXIST");
            parameters.Add("ProductId", productId);
            parameters.Add("SMCProductId", smcProductId);
            parameters.Add("SMCProductItemId", itemId);
            parameters.Add("EntryDate", date.Date);

            return await conn.QueryFirstOrDefaultAsync<SMCConfig>(
                "dbo.sp_SMCConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(SMCConfig model)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("SMCProductId", model.SMCProductId);
            parameters.Add("SMCProductItemId", model.SMCProductItemId);
            parameters.Add("EntryDate", model.EntryDate.Date);
            parameters.Add("ConfigValue", model.ConfigValue);
            parameters.Add("IsChecked", model.IsChecked);
            parameters.Add("EntryMode", model.EntryMode);
            parameters.Add("Remarks", model.Remarks);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("CreatedDate", model.CreatedDate);
            parameters.Add("CreatedBy", model.CreatedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(SMCConfig model)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("SMCConfigId", model.SMCConfigId);
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("SMCProductId", model.SMCProductId);
            parameters.Add("SMCProductItemId", model.SMCProductItemId);
            parameters.Add("EntryDate", model.EntryDate.Date);
            parameters.Add("ConfigValue", model.ConfigValue);
            parameters.Add("IsChecked", model.IsChecked);
            parameters.Add("EntryMode", model.EntryMode);
            parameters.Add("Remarks", model.Remarks);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("ModifiedDate", model.ModifiedDate);
            parameters.Add("ModifiedBy", model.ModifiedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id, DateTime? deletedDate, string? deletedBy)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("SMCConfigId", id);
            parameters.Add("DeletedDate", deletedDate);
            parameters.Add("DeletedBy", deletedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("SMCConfigId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}