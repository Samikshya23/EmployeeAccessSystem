using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly string _connectionString;

        public ConfigurationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Configuration>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<Configuration>(
                "dbo.sp_Configuration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Configuration>> GetByProductIdAsync(int productId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYPRODUCTID");
            parameters.Add("ProductId", productId);

            return await conn.QueryAsync<Configuration>(
                "dbo.sp_Configuration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Configuration> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("ConfigurationId", id);

            return await conn.QueryFirstOrDefaultAsync<Configuration>(
                "dbo.sp_Configuration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(Configuration model)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "INSERT");
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("ParentConfigurationId", model.ParentConfigurationId);
            parameters.Add("ConfigurationName", model.ConfigurationName);
            parameters.Add("NodeType", model.NodeType);
            parameters.Add("InputType", model.InputType);
            parameters.Add("SortOrder", model.SortOrder);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("CreatedBy", model.CreatedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_Configuration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(Configuration model)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("ConfigurationId", model.ConfigurationId);
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("ParentConfigurationId", model.ParentConfigurationId);
            parameters.Add("ConfigurationName", model.ConfigurationName);
            parameters.Add("NodeType", model.NodeType);
            parameters.Add("InputType", model.InputType);
            parameters.Add("SortOrder", model.SortOrder);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("ModifiedBy", model.ModifiedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_Configuration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("ConfigurationId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_Configuration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("ConfigurationId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_Configuration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}