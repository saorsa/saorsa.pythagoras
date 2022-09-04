namespace Saorsa.Pythagoras.Domain.Configuration;

public class PythagorasAuthorizationConfiguration
{
    public PythagorasAuthorizationMode DefaultMode { get; set; } = PythagorasAuthorizationMode.InProc;
}
