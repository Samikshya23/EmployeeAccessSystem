using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class ProductConfigurationRepository : IProductConfigurationRepository
    {
        private readonly string _connectionString;

        public ProductConfigurationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<ProductConfiguration>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<ProductConfiguration>(
                "dbo.sp_ProductConfiguration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductConfiguration>> GetByProductIdAsync(int productId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYPRODUCT");
            parameters.Add("ProductId", productId);

            return await conn.QueryAsync<ProductConfiguration>(
                "dbo.sp_ProductConfiguration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(ProductConfiguration model)
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
                "dbo.sp_ProductConfiguration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteByProductAsync(int productId, string deletedBy)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETEBYPRODUCT");
            parameters.Add("ProductId", productId);
            parameters.Add("DeletedBy", deletedBy);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_ProductConfiguration_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}