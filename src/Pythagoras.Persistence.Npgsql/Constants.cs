namespace Saorsa.Pythagoras.Persistence.Npgsql;

public static class Constants
{
    public static class EnvironmentVariables
    {
        public static readonly string Server = "PYTHAGORAS_POSTGRES_SERVER";
        public static readonly string Port = "PYTHAGORAS_POSTGRES_PORT";
        public static readonly string Database = "PYTHAGORAS_POSTGRES_DATABASE";
        public static readonly string UseProcessSecurity = "PYTHAGORAS_POSTGRES_USE_PROCESS_SECURITY";
        public static readonly string User = "PYTHAGORAS_POSTGRES_USER";
        public static readonly string Password = "PYTHAGORAS_POSTGRES_PASSWORD";
    }
}
