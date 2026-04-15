using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EmployeeAccessSystem.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeAccessSystem.Repositories
{
    public class PingProductItemValueRepository : IPingProductItemValueRepository
    {
        private readonly string _connectionString;

        public PingProductItemValueRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<PingProductItemValue>> GetAllAsync()
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETALL");

            return await conn.QueryAsync<PingProductItemValue>(
                "dbo.sp_PingProductItemValue_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<PingProductItemValue>> GetByItemIdAsync(int pingProductItemId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "GETBYITEM");
            parameters.Add("PingProductItemId", pingProductItemId);

            return await conn.QueryAsync<PingProductItemValue>(
                "dbo.sp_PingProductItemValue_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PingProductItemValue> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            string sql = @"
                SELECT TOP 1
                    ppiv.PingProductItemValueId,
                    ppiv.PingProductItemId,
                    ppiv.PingProductFieldId,
                    ppiv.FieldValue,
                    ppiv.IsActive,
                    ppf.FieldName,
                    pp.PingProductName,
                    ppi.ItemName
                FROM PingProductItemValues ppiv
                INNER JOIN PingProductFields ppf ON ppiv.PingProductFieldId = ppf.PingProductFieldId
                INNER JOIN PingProductItems ppi ON ppiv.PingProductItemId = ppi.PingProductItemId
                INNER JOIN PingProducts pp ON ppi.PingProductId = pp.PingProductId
                WHERE ppiv.PingProductItemValueId = @PingProductItemValueId";

            return await conn.QueryFirstOrDefaultAsync<PingProductItemValue>(
                sql,
                new { PingProductItemValueId = id }
            );
        }

        public async Task<PingProductItemValue> CheckDuplicateAsync(int pingProductItemId, int pingProductFieldId)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "CHECKDUPLICATE");
            parameters.Add("PingProductItemId", pingProductItemId);
            parameters.Add("PingProductFieldId", pingProductFieldId);

            return await conn.QueryFirstOrDefaultAsync<PingProductItemValue>(
                "dbo.sp_PingProductItemValue_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddAsync(PingProductItemValue pingProductItemValue)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "ADD");
            parameters.Add("PingProductItemId", pingProductItemValue.PingProductItemId);
            parameters.Add("PingProductFieldId", pingProductItemValue.PingProductFieldId);
            parameters.Add("FieldValue", pingProductItemValue.FieldValue);
            parameters.Add("IsActive", pingProductItemValue.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductItemValue_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateAsync(PingProductItemValue pingProductItemValue)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "UPDATE");
            parameters.Add("PingProductItemValueId", pingProductItemValue.PingProductItemValueId);
            parameters.Add("FieldValue", pingProductItemValue.FieldValue);
            parameters.Add("IsActive", pingProductItemValue.IsActive);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductItemValue_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = GetConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Flag", "DELETE");
            parameters.Add("PingProductItemValueId", id);

            return await conn.ExecuteScalarAsync<int>(
                "dbo.sp_PingProductItemValue_Manage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}