using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmployeeAccessSystem.Repositories
{
    
    public class CoreDbConnection : ICoreDbConnection
    {
        public string ConnectionString { get; }
        private readonly ILogger<CoreDbConnection> _logger;

        public CoreDbConnection(IConfiguration configuration, ILogger<CoreDbConnection> logger)
        {
            _logger = logger;

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("Database connection string not found in configuration.");
                throw new InvalidOperationException("Database connection string not found.");
            }

            ConnectionString = connectionString;
        }
    }
}
