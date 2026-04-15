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
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<PingConfig>(
                "dbo.sp_PingConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingConfig?> GetByIdAsync(int id)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("PingConfigId", id);

            return await conn.QueryFirstOrDefaultAsync<PingConfig>(
                "dbo.sp_PingConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingConfig?> GetExistingAsync(int productId, int pingProductId, int itemId, DateTime date)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKEXIST");
            parameters.Add("ProductId", productId);
            parameters.Add("PingProductId", pingProductId);
            parameters.Add("PingProductItemId", itemId);
            parameters.Add("EntryDate", date.Date);

            return await conn.QueryFirstOrDefaultAsync<PingConfig>(
                "dbo.sp_PingConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(PingConfig model)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("PingProductId", model.PingProductId);
            parameters.Add("PingProductItemId", model.PingProductItemId);
            parameters.Add("EntryDate", model.EntryDate.Date);
            parameters.Add("ConfigValue", model.ConfigValue);
            parameters.Add("IsChecked", model.IsChecked);
            parameters.Add("EntryMode", model.EntryMode);
            parameters.Add("Remarks", model.Remarks);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("CreatedDate", model.CreatedDate);
            parameters.Add("CreatedBy", model.CreatedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(PingConfig model)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("PingConfigId", model.PingConfigId);
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("PingProductId", model.PingProductId);
            parameters.Add("PingProductItemId", model.PingProductItemId);
            parameters.Add("EntryDate", model.EntryDate.Date);
            parameters.Add("ConfigValue", model.ConfigValue);
            parameters.Add("IsChecked", model.IsChecked);
            parameters.Add("EntryMode", model.EntryMode);
            parameters.Add("Remarks", model.Remarks);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("ModifiedDate", model.ModifiedDate);
            parameters.Add("ModifiedBy", model.ModifiedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id, DateTime? deletedDate, string? deletedBy)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("PingConfigId", id);
            parameters.Add("DeletedDate", deletedDate);
            parameters.Add("DeletedBy", deletedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using SqlConnection conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("PingConfigId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingConfig_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}