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

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<SMCProductItem>(
                "dbo.sp_SMCProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<SMCProductItem>> GetActiveAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETACTIVE");

            return await conn.QueryAsync<SMCProductItem>(
                "dbo.sp_SMCProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<SMCProductItem>> GetByProductAsync(int smcProductId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYPRODUCT");
            parameters.Add("SMCProductId", smcProductId);

            return await conn.QueryAsync<SMCProductItem>(
                "dbo.sp_SMCProductItem_Manage",
                parameters,
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

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("SMCProductId", model.SMCProductId);
            parameters.Add("ItemName", model.ItemName);
            parameters.Add("IsActive", model.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(SMCProductItem model)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("SMCProductItemId", model.SMCProductItemId);
            parameters.Add("SMCProductId", model.SMCProductId);
            parameters.Add("ItemName", model.ItemName);
            parameters.Add("IsActive", model.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("SMCProductItemId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("SMCProductItemId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_SMCProductItem_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}