namespace Saorsa.Pythagoras.Domain.Configuration;

public class PythagorasInProcAuthenticationConfiguration
{
    public string AuthenticationScheme { get; set; } = "InProc";
    
    public string UserClaimType { get; set; } = "user";

    public string RoleClaimType { get; set; } = "group";

    public bool Enabled { get; set; } = true;
}
