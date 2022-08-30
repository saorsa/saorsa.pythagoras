namespace Saorsa.Pythagoras.Domain.Model;

public class IdentityContext
{
    public string User { get; }
    
    public IEnumerable<string> Groups { get; }

    public IdentityContext(string user, IEnumerable<string>? groups = null)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        Groups = groups ?? Array.Empty<string>();
    }
}
