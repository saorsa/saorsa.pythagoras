using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saorsa.Pythagoras.Domain.Auth;
using Saorsa.Pythagoras.Persistence;

namespace Saorsa.Pythagoras.Domain.Business.Concrete;

public class DefaultPythagorasPersistenceManager
{
    public IPythagorasSessionManager IdProvider { get; }
    public PythagorasDbContext DbContext { get; }
    public ILogger<DefaultPythagorasPersistenceManager> Logger { get; }

    public DefaultPythagorasPersistenceManager(
        IPythagorasSessionManager idProvider,
        PythagorasDbContext dbContext,
        ILogger<DefaultPythagorasPersistenceManager> logger)
    {
        IdProvider = idProvider;
        DbContext = dbContext;
        Logger = logger;
    }

    public Task MigrateDatabaseAsync(CancellationToken cancellationToken)
    {
        return DbContext.Database.MigrateAsync(cancellationToken);
    }
}
