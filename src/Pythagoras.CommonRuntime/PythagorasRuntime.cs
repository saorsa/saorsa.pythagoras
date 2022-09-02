using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Saorsa.Pythagoras.CommonRuntime;

public static class PythagorasRuntime
{
    public static bool IsAspNetCore =>
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null;
    
    public static string AppEnvironment
        => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? 
           (Environment.GetEnvironmentVariable("PYTHAGORAS_ENVIRONMENT") ?? "Development");

    public static ILogger GetConfiguredLogger()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile(
                $"appsettings.{AppEnvironment}.json", 
                true)
            .Build();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        logger.Information(
            "Application logger initialized for environment {Environment}", AppEnvironment);
        return logger;
    }
}
