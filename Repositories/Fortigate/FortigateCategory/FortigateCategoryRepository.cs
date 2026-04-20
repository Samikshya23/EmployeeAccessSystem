using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class FortigateCategoryRepository : IFortigateCategoryRepository
    {
        private readonly string _connectionString;

        public FortigateCategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<FortigateCategory>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<FortigateCategory>(
                "dbo.sp_FortigateCategory_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<FortigateCategory>> GetActiveAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETACTIVE");

            return await conn.QueryAsync<FortigateCategory>(
                "dbo.sp_FortigateCategory_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<FortigateCategory> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("FortigateCategoryId", id);

            return await conn.QueryFirstOrDefaultAsync<FortigateCategory>(
                "dbo.sp_FortigateCategory_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(FortigateCategory model)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "INSERT");
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("CategoryName", model.CategoryName);
            parameters.Add("DisplayOrder", model.DisplayOrder);
            parameters.Add("IsActive", model.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_FortigateCategory_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(FortigateCategory model)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("FortigateCategoryId", model.FortigateCategoryId);
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("CategoryName", model.CategoryName);
            parameters.Add("DisplayOrder", model.DisplayOrder);
            parameters.Add("IsActive", model.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_FortigateCategory_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("FortigateCategoryId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_FortigateCategory_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("FortigateCategoryId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_FortigateCategory_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}