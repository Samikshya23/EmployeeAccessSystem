using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<int> CreateAsync(Account account)
        {
            using var conn = GetConnection();

            string sql = @"
                INSERT INTO Accounts (FullName, Email, PasswordHash, PasswordSalt)
                VALUES (@FullName, @Email, @PasswordHash, @PasswordSalt);

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            return await conn.ExecuteScalarAsync<int>(sql, account);
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            using var conn = GetConnection();

            string sql = @"
                SELECT 
                    a.AccountId,
                    a.FullName,
                    a.Email,
                    a.PasswordHash,
                    a.PasswordSalt,
                    r.RoleName
                FROM Accounts a
                INNER JOIN UserRoles ur ON a.AccountId = ur.AccountId
                INNER JOIN Roles r ON ur.RoleId = r.RoleId
                WHERE a.Email = @Email
            ";

            return await conn.QueryFirstOrDefaultAsync<Account>(sql, new { Email = email });
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            string sql = @"
                SELECT 
                    a.AccountId,
                    a.FullName,
                    a.Email,
                    a.PasswordHash,
                    a.PasswordSalt,
                    r.RoleName
                FROM Accounts a
                INNER JOIN UserRoles ur ON a.AccountId = ur.AccountId
                INNER JOIN Roles r ON ur.RoleId = r.RoleId
                WHERE a.AccountId = @Id
            ";

            return await conn.QueryFirstOrDefaultAsync<Account>(sql, new { Id = id });
        }

        public async Task<int> UpdateAsync(Account account)
        {
            using var conn = GetConnection();

            string sql = @"
                UPDATE Accounts
                SET FullName = @FullName,
                    Email = @Email
                WHERE AccountId = @AccountId;
            ";

            return await conn.ExecuteAsync(sql, account);
        }

        public async Task<int> DeleteAsync(int accountId)
        {
            using var conn = GetConnection();

            string deleteUserRolesSql = @"DELETE FROM UserRoles WHERE AccountId = @Id;";
            await conn.ExecuteAsync(deleteUserRolesSql, new { Id = accountId });

            string deleteAccountSql = @"DELETE FROM Accounts WHERE AccountId = @Id;";
            return await conn.ExecuteAsync(deleteAccountSql, new { Id = accountId });
        }

        public async Task<int> AssignRoleAsync(int accountId, int roleId)
        {
            using var conn = GetConnection();

            string sql = @"
                INSERT INTO UserRoles (AccountId, RoleId)
                VALUES (@AccountId, @RoleId)
            ";

            return await conn.ExecuteAsync(sql, new
            {
                AccountId = accountId,
                RoleId = roleId
            });
        }

        public async Task<int> UpdateRoleAsync(int accountId, int roleId)
        {
            using var conn = GetConnection();

            string deleteOldRoleSql = @"
                DELETE FROM UserRoles
                WHERE AccountId = @AccountId;
            ";

            await conn.ExecuteAsync(deleteOldRoleSql, new { AccountId = accountId });

            string insertNewRoleSql = @"
                INSERT INTO UserRoles (AccountId, RoleId)
                VALUES (@AccountId, @RoleId);
            ";

            return await conn.ExecuteAsync(insertNewRoleSql, new
            {
                AccountId = accountId,
                RoleId = roleId
            });
        }
    }
}