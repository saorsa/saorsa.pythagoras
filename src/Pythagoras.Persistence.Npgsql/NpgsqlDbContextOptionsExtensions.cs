using Microsoft.EntityFrameworkCore;

namespace Saorsa.Pythagoras.Persistence.Npgsql;

public static class NpgsqlDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UsePythagorasWithNpgSql(
        this DbContextOptionsBuilder builder,
        string? connectionString = null)
    {
        connectionString ??= NpgsqlConnectionStringHelper
            .BuildConnectionStringWithEnvironmentVariables(
                Constants.EnvironmentVariables.Server,
                Constants.EnvironmentVariables.Port,
                Constants.EnvironmentVariables.Database,
                Constants.EnvironmentVariables.UseProcessSecurity,
                Constants.EnvironmentVariables.User,
                Constants.EnvironmentVariables.Password
            );

        return builder.UseNpgsql(connectionString, optionsBuilder =>
        {
            optionsBuilder.MigrationsAssembly(typeof(NpgsqlConnectionStringHelper).Assembly.GetName().Name);
        });
    }
}
