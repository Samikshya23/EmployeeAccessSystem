using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class PingProductItemRepository : IPingProductItemRepository
    {
        private readonly string _connectionString;

        public PingProductItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<PingProductItem>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<PingProductItem>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<PingProductItem>> GetActiveAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETACTIVE");

            return await conn.QueryAsync<PingProductItem>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<PingProductItem>> GetByPingProductIdAsync(int pingProductId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYPINGPRODUCT");
            parameters.Add("PingProductId", pingProductId);

            return await conn.QueryAsync<PingProductItem>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProductItem> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("PingProductItemId", id);

            return await conn.QueryFirstOrDefaultAsync<PingProductItem>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProductItem> CheckDuplicateAsync(int pingProductId, string itemName)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKDUPLICATE");
            parameters.Add("PingProductId", pingProductId);
            parameters.Add("ItemName", itemName);

            return await conn.QueryFirstOrDefaultAsync<PingProductItem>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProductItem> CheckDuplicateForUpdateAsync(int pingProductItemId, int pingProductId, string itemName)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKDUPLICATEFORUPDATE");
            parameters.Add("PingProductItemId", pingProductItemId);
            parameters.Add("PingProductId", pingProductId);
            parameters.Add("ItemName", itemName);

            return await conn.QueryFirstOrDefaultAsync<PingProductItem>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(PingProductItem pingProductItem)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("PingProductId", pingProductItem.PingProductId);
            parameters.Add("ItemName", pingProductItem.ItemName);
            parameters.Add("IsActive", pingProductItem.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(PingProductItem pingProductItem)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("PingProductItemId", pingProductItem.PingProductItemId);
            parameters.Add("PingProductId", pingProductItem.PingProductId);
            parameters.Add("ItemName", pingProductItem.ItemName);
            parameters.Add("IsActive", pingProductItem.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("PingProductItemId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("PingProductItemId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}