using Microsoft.Extensions.Logging;
using Npgsql;

namespace Saorsa.Pythagoras.Persistence.Npgsql;

public class NpgsqlConnectionStringHelper
{
    public const int DefaultNpgsqlPort = 5432;

    public static string BuildConnectionStringWithEnvironmentVariables(
        string envVariableServer,
        string envVariablePort,
        string envVariableDatabase,
        string envVariableUseProcessSecurity,
        string envVariableUser,   
        string envVariablePassword,
        ILogger? logger = null)
    {
        if (logger == null)
        {
            using ILoggerFactory loggerFactory =
                LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole(options =>
                    {
                        options.SingleLine = true;
                        options.IncludeScopes = true;
                        options.TimestampFormat = "yyyyMMdd@HHmmss ";
                    }));

            logger = loggerFactory.CreateLogger<NpgsqlConnectionStringHelper>();
        }

        var postgreServer = Environment.GetEnvironmentVariable(envVariableServer);
        var postgrePort = DefaultNpgsqlPort;
        var postgrePortString = Environment.GetEnvironmentVariable(envVariablePort);
        var postgreDatabase = Environment.GetEnvironmentVariable(envVariableDatabase);
        var postgreUseProcessSecurity = false;
        var postgreUseProcessSecurityString = Environment.GetEnvironmentVariable(envVariableUseProcessSecurity);
        var postgreUser = Environment.GetEnvironmentVariable(envVariableUser);
        var postgrePassword = Environment.GetEnvironmentVariable(envVariablePassword);

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
        
        if (postgreServer == null)
        {
            var errorMessage = $"Environment variable '{envVariableServer}' is not set";
            logger.LogError(errorMessage);
            throw new NpgsqlConnectionStringHelperException(envVariableServer, errorMessage);
        }

        connectionStringBuilder.Host = postgreServer;
        
        if (postgreDatabase == null)
        {
            var errorMessage = $"Environment variable '{envVariableDatabase}' is not set";
            logger.LogError(errorMessage);
            throw new NpgsqlConnectionStringHelperException(envVariableDatabase, errorMessage);
        }

        connectionStringBuilder.Database = postgreDatabase;

        if (postgrePortString != null)
        {
            if (int.TryParse(postgrePortString, out var zeusPortParsed))
            {
                postgrePort = zeusPortParsed;
                connectionStringBuilder.Port = postgrePort;
            }
        }

        if (postgreUseProcessSecurityString != null)
        {
            if (bool.TryParse(postgreUseProcessSecurityString, out var zeusProcessSecurityParsed))
            {
                postgreUseProcessSecurity = zeusProcessSecurityParsed;
                connectionStringBuilder.IntegratedSecurity = postgreUseProcessSecurity;
            }
        }
        
        var message = $@"Probing environment variables for connection string: 
${envVariableServer}: {postgreServer}
${envVariablePort}: {postgrePort}
${envVariableDatabase}: {postgreDatabase}
${envVariableUseProcessSecurity}: {postgreUseProcessSecurity}
${envVariableUser}: {postgreUser}
${envVariablePassword}: {postgrePassword}";

        logger.LogDebug(message);
        
        if (postgreUseProcessSecurity)
        {
            logger.LogDebug("Postgres database: Using process security");
            return connectionStringBuilder.ConnectionString;
        }

        if (postgreUser == null)
        {
            var errorMessage = $"Environment variable {envVariableUser} is not set";
            logger.LogError(errorMessage);
            throw new NpgsqlConnectionStringHelperException(envVariableUser, errorMessage);
        }

        connectionStringBuilder.Username = postgreUser;

        if (postgrePassword == null)
        {
            var errorMessage = $"Environment variable {envVariablePassword} is not set";
            logger.LogError(errorMessage);
            throw new NpgsqlConnectionStringHelperException(envVariablePassword, errorMessage);
        }

        connectionStringBuilder.Password = postgrePassword;

        logger.LogDebug("Zeus Postgres database: Using username and password");
        return connectionStringBuilder.ConnectionString;
    }
}