namespace Saorsa.Pythagoras.Domain.Configuration;

public class PythagorasAuthenticationConfiguration
{
    public string? DefaultAuthenticationScheme { get; set; }

    public PythagorasInProcAuthenticationConfiguration InProc { get; set; } = new();
}
