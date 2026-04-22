using System.Data;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class FortigateConfigRepositories : IFortigateConfigRepositories
    {
        private readonly string _connectionString;

        public FortigateConfigRepositories(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public async Task<IEnumerable<FortigateConfig>> GetAllAsync()
        {
            using var conn = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");
            return await conn.QueryAsync<FortigateConfig>("dbo.sp_FortigateConfig_Manage",parameters,commandType: CommandType.StoredProcedure);
        }
        public async Task<FortigateConfig?> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("FortigateConfigId", id);
            return await conn.QueryFirstOrDefaultAsync<FortigateConfig>("dbo.sp_FortigateConfig_Manage",parameters,commandType: CommandType.StoredProcedure );
        }
        public async Task<FortigateConfig?> GetExistingAsync(int productId, int categoryId, int itemId, DateTime date)
        {
            using var conn = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKEXIST");
            parameters.Add("ProductId", productId);
            parameters.Add("FortigateCategoryId", categoryId);
            parameters.Add("FortigateItemId", itemId);
            parameters.Add("EntryDate", date.Date);
            return await conn.QueryFirstOrDefaultAsync<FortigateConfig>("dbo.sp_FortigateConfig_Manage",parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> AddAsync(FortigateConfig model)
        {
            using var conn = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("FortigateCategoryId", model.FortigateCategoryId);
            parameters.Add("FortigateItemId", model.FortigateItemId);
            parameters.Add("EntryDate", model.EntryDate.Date);
            parameters.Add("ConfigValue", model.ConfigValue);
            parameters.Add("Remarks", model.Remarks);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("CreatedDate", model.CreatedDate);
            parameters.Add("CreatedBy", model.CreatedBy);
            return await conn.ExecuteScalarAsync<int>( "dbo.sp_FortigateConfig_Manage",parameters,commandType: CommandType.StoredProcedure );
        }
        public async Task<int> UpdateAsync(FortigateConfig model)
        {
            using var conn = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("FortigateConfigId", model.FortigateConfigId);
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("FortigateCategoryId", model.FortigateCategoryId);
            parameters.Add("FortigateItemId", model.FortigateItemId);
            parameters.Add("EntryDate", model.EntryDate.Date);
            parameters.Add("ConfigValue", model.ConfigValue);
            parameters.Add("Remarks", model.Remarks);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("ModifiedDate", model.ModifiedDate);
            parameters.Add("ModifiedBy", model.ModifiedBy);
            return await conn.ExecuteScalarAsync<int>("dbo.sp_FortigateConfig_Manage",parameters,commandType: CommandType.StoredProcedure );
        }
        public async Task<int> DeleteAsync(int id, DateTime? deletedDate, string? deletedBy)
        {
            using var conn = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("FortigateConfigId", id);
            parameters.Add("DeletedDate", deletedDate);
            parameters.Add("DeletedBy", deletedBy);
            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_FortigateConfig_Manage", parameters, commandType: CommandType.StoredProcedure );
        }
        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("FortigateConfigId", id);
            return await conn.ExecuteScalarAsync<int>( "dbo.sp_FortigateConfig_Manage", parameters, commandType: CommandType.StoredProcedure );
        }
    }
}