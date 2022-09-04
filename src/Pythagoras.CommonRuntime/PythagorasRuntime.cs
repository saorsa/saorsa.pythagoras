using Microsoft.Extensions.Configuration;
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

    public static IConfigurationRoot GetConfigurationFromAppSettings()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile(
                $"appsettings.{AppEnvironment}.json",
                true)
            .Build();
        return configuration;
    }

    public static ILogger GetConfiguredLogger()
    {
        var configuration = GetConfigurationFromAppSettings();
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        logger.Information(
            "Application logger initialized for environment {Environment}", AppEnvironment);
        return logger;
    }
}
