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
            SqlConnection conn = new SqlConnection(_connectionString);
            return conn;
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
            string sql = @"SELECT * FROM Accounts WHERE Email = @Email";
            return await conn.QueryFirstOrDefaultAsync<Account>(sql, new { Email = email });
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            string sql = @"SELECT * FROM Accounts WHERE AccountId  =@Id";
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
            string sql = "DELETE FROM dbo.Accounts WHERE AccountId = @Id";
            return await conn.ExecuteAsync(sql, new { Id = accountId });
        }

    }
}
