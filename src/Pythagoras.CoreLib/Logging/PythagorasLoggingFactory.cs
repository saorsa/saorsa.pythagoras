using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Extensions.Logging;
using MELLogger = Microsoft.Extensions.Logging.ILogger;

namespace Pythagoras.Logging;

public class PythagorasLoggingFactory : ILoggerFactory
{
    private readonly SerilogLoggerProvider _provider;

    public PythagorasLoggingFactory(bool dispose = false)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        _provider = new SerilogLoggerProvider(logger, dispose);
    }

    public void Dispose() => _provider.Dispose();

    public MELLogger CreateLogger(string categoryName)
    {
        return _provider.CreateLogger(categoryName);
    }

    public void AddProvider(ILoggerProvider provider)
    {
        SelfLog.WriteLine("Ignoring added logger provider {0}", provider);
    }
}
