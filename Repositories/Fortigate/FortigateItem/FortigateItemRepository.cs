using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class FortigateItemRepositories : IFortigateItemRepositories
    {
        private readonly string _connectionString;

        public FortigateItemRepositories(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<FortigateItem>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<FortigateItem>(
                "dbo.sp_FortigateItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<FortigateItem>> GetActiveAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETACTIVE");

            return await conn.QueryAsync<FortigateItem>(
                "dbo.sp_FortigateItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<FortigateItem>> GetByCategoryAsync(int fortigateCategoryId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYCATEGORY");
            parameters.Add("FortigateCategoryId", fortigateCategoryId);

            return await conn.QueryAsync<FortigateItem>(
                "dbo.sp_FortigateItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<FortigateItem> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("FortigateItemId", id);

            return await conn.QueryFirstOrDefaultAsync<FortigateItem>(
                "dbo.sp_FortigateItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(FortigateItem model)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "INSERT");
            parameters.Add("FortigateCategoryId", model.FortigateCategoryId);
            parameters.Add("ItemName", model.ItemName);
            parameters.Add("IsActive", model.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_FortigateItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(FortigateItem model)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("FortigateItemId", model.FortigateItemId);
            parameters.Add("FortigateCategoryId", model.FortigateCategoryId);
            parameters.Add("ItemName", model.ItemName);
            parameters.Add("IsActive", model.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_FortigateItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("FortigateItemId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_FortigateItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("FortigateItemId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_FortigateItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}