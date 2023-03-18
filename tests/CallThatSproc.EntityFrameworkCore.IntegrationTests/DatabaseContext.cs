using Microsoft.EntityFrameworkCore;

namespace CallThatSproc.EntityFrameworkCore.IntegrationTests;

public class DatabaseContext : DbContext
{
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
