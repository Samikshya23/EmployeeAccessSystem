namespace EmployeeAccessSystem.Repositories
{
    /// <summary>
    /// Provides the database connection string.
    /// </summary>
    public interface ICoreDbConnection
    {
        string ConnectionString { get; }
    }
}
