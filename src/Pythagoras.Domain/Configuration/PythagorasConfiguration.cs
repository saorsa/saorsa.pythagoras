namespace Saorsa.Pythagoras.Domain.Configuration;

public class PythagorasConfiguration
{
    public PythagorasAuthenticationConfiguration Authentication { get; set; } = new();
}

public static class PythagorasConfigurationExtensions
{
    public static PythagorasConfiguration InvalidateFrom(
        this PythagorasConfiguration source, PythagorasConfiguration other)
    {
        source.Authentication = other.Authentication;
        return source;
    }
}
