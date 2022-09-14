using Saorsa.Pythagoras.Domain.Model;

namespace Saorsa.Pythagoras.RestApi.Model;

public class DTOIdentityContext
{
    public IdentitySession Session { get; }
    public IdentityClaim[] Claims { get; }

    public DTOIdentityContext(IdentitySession session, IEnumerable<IdentityClaim> claims)
    {
        Session = session;
        Claims = claims.ToArray();
    }
}
