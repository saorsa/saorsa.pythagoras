using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Saorsa.Pythagoras.Persistence.Npgsql;

public class PythagorasDbContextFactory : IDesignTimeDbContextFactory<PythagorasDbContext>
{
    public PythagorasDbContext CreateDbContext(string[] args)
    {
        using ILoggerFactory loggerFactory =
            LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.IncludeScopes = true;
                    options.TimestampFormat = "yyyyMMdd@HHmmss ";
                }));

        var logger = loggerFactory.CreateLogger<NpgsqlConnectionStringHelper>();
        try
        {
            var optionsBuilder = new DbContextOptionsBuilder<PythagorasDbContext>();

            optionsBuilder.UsePythagorasWithNpgSql();
        
            var result = new PythagorasDbContext(optionsBuilder.Options);

            return result;
        }
        catch (ApplicationException ae)
        {
            logger.LogCritical(ae.Message);
            throw;
        }
        catch (Exception e)
        {
            logger.LogCritical(e.Message);
            throw;
        }
    }
}
