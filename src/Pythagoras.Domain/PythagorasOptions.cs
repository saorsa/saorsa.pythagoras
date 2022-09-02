namespace Saorsa.Pythagoras.Domain;

public class PythagorasOptions
{
    public string SuperAdminUser { get; set; } = "root";

    public PythagorasAuthorizationOptions Authorization { get; set; } = new();
}

public static class PythagorasOptionsExtensions
{
    public static PythagorasOptions InvalidateFrom(this PythagorasOptions source, PythagorasOptions other)
    {
        source.SuperAdminUser = other.SuperAdminUser;
        source.Authorization = other.Authorization;
        return source;
    }
}
