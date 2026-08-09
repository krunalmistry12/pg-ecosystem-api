using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PGManagementSystem.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("AIVEN_MYSQL_CONNECTION");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "AIVEN_MYSQL_CONNECTION environment variable is not set.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseMySql(
            connectionString,
            new MySqlServerVersion(new Version(8, 4, 0))
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}