namespace Saorsa.Pythagoras.Domain.Model;

public class IdentityContext
{
    public string AuthenticationScheme { get; }
    
    public string User { get; }
    
    public IEnumerable<string> Groups { get; }

    public IdentityContext(
        string authenticationScheme,
        string user,
        IEnumerable<string>? groups = null)
    {
        AuthenticationScheme = authenticationScheme ?? throw new ArgumentNullException(nameof(authenticationScheme));
        User = user ?? throw new ArgumentNullException(nameof(user));
        Groups = groups ?? Array.Empty<string>();
    }
}
