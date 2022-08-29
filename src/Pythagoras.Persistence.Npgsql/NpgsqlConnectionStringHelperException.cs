using System.Runtime.Serialization;

namespace Saorsa.Pythagoras.Persistence.Npgsql;

public class NpgsqlConnectionStringHelperException : Exception
{
    public string EnvironmentVariableKey { get; }

    public NpgsqlConnectionStringHelperException(string envVariableKey, string message)
        : base(message)
    {
        EnvironmentVariableKey = envVariableKey;
    }

    public NpgsqlConnectionStringHelperException(string envVariableKey, string message, Exception inner)
        : base(message, inner)
    {
        EnvironmentVariableKey = envVariableKey;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(EnvironmentVariableKey), EnvironmentVariableKey, typeof(string)); 
    }
}
