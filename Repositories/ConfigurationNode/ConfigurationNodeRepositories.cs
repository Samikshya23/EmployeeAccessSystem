using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class ConfigurationNodeRepositories : IConfigurationNodeRepositories
    {
        private readonly string _connectionString;

        public ConfigurationNodeRepositories(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<ConfigurationNode>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<ConfigurationNode>(
                "dbo.sp_ConfigurationNode_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ConfigurationNode>> GetByProductIdAsync(int productId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALLBYPRODUCT");
            parameters.Add("ProductId", productId);

            return await conn.QueryAsync<ConfigurationNode>(
                "dbo.sp_ConfigurationNode_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ConfigurationNode> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("NodeId", id);

            return await conn.QueryFirstOrDefaultAsync<ConfigurationNode>(
                "dbo.sp_ConfigurationNode_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(ConfigurationNode model)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("ParentNodeId", model.ParentNodeId);
            parameters.Add("NodeName", model.NodeName);
            parameters.Add("NodeType", model.NodeType);
            parameters.Add("InputType", model.InputType);
            parameters.Add("SortOrder", model.SortOrder);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("CreatedBy", model.CreatedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_ConfigurationNode_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(ConfigurationNode model)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("NodeId", model.NodeId);
            parameters.Add("ProductId", model.ProductId);
            parameters.Add("ParentNodeId", model.ParentNodeId);
            parameters.Add("NodeName", model.NodeName);
            parameters.Add("NodeType", model.NodeType);
            parameters.Add("InputType", model.InputType);
            parameters.Add("IsActive", model.IsActive);
            parameters.Add("ModifiedBy", model.ModifiedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_ConfigurationNode_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("NodeId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_ConfigurationNode_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}