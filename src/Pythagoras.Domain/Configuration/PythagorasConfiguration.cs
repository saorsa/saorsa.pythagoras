namespace Saorsa.Pythagoras.Domain.Configuration;

public class PythagorasConfiguration
{
    public string SuperAdminUser { get; set; } = "root";

    public PythagorasAuthorizationConfiguration Authorization { get; set; } = new();
}

public static class PythagorasConfigurationExtensions
{
    public static PythagorasConfiguration InvalidateFrom(
        this PythagorasConfiguration source, PythagorasConfiguration other)
    {
        source.SuperAdminUser = other.SuperAdminUser;
        source.Authorization = other.Authorization;
        return source;
    }
}
