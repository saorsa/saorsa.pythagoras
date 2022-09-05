namespace Saorsa.Pythagoras.Domain.Configuration;

public class PythagorasAuthorizationConfiguration
{
    public PythagorasAuthorizationMode DefaultMode { get; set; } = PythagorasAuthorizationMode.InProc;

    public string UserClaimType { get; set; } = "user";

    public string RoleClaimType { get; set; } = "group";
}
