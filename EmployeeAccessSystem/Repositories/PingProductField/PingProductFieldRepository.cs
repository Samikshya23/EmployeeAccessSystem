using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class PingProductFieldRepository : IPingProductFieldRepository
    {
        private readonly string _connectionString;

        public PingProductFieldRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<PingProductField>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<PingProductField>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<PingProductField>> GetActiveAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETACTIVE");

            return await conn.QueryAsync<PingProductField>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<PingProductField>> GetByPingProductIdAsync(int pingProductId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYPINGPRODUCT");
            parameters.Add("PingProductId", pingProductId);

            return await conn.QueryAsync<PingProductField>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProductField> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYID");
            parameters.Add("PingProductFieldId", id);

            return await conn.QueryFirstOrDefaultAsync<PingProductField>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProductField> CheckDuplicateAsync(int pingProductId, string fieldName)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKDUPLICATE");
            parameters.Add("PingProductId", pingProductId);
            parameters.Add("FieldName", fieldName);

            return await conn.QueryFirstOrDefaultAsync<PingProductField>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProductField> CheckDuplicateForUpdateAsync(int pingProductFieldId, int pingProductId, string fieldName)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKDUPLICATEFORUPDATE");
            parameters.Add("PingProductFieldId", pingProductFieldId);
            parameters.Add("PingProductId", pingProductId);
            parameters.Add("FieldName", fieldName);

            return await conn.QueryFirstOrDefaultAsync<PingProductField>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(PingProductField pingProductField)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("PingProductId", pingProductField.PingProductId);
            parameters.Add("FieldName", pingProductField.FieldName);
            parameters.Add("DisplayOrder", pingProductField.DisplayOrder);
            parameters.Add("IsActive", pingProductField.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(PingProductField pingProductField)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("PingProductFieldId", pingProductField.PingProductFieldId);
            parameters.Add("PingProductId", pingProductField.PingProductId);
            parameters.Add("FieldName", pingProductField.FieldName);
            parameters.Add("DisplayOrder", pingProductField.DisplayOrder);
            parameters.Add("IsActive", pingProductField.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("PingProductFieldId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "TOGGLE");
            parameters.Add("PingProductFieldId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductField_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}