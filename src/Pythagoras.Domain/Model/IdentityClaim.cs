using System.Security.Claims;

namespace Saorsa.Pythagoras.Domain.Model;

public class IdentityClaim
{
    public string Type { get; }
    
    public string ValueType { get; }
    
    public string Issuer { get; }
    
    public string Value { get; }

    public IdentityClaim(Claim claim)
    {
        Type = claim.Type;
        ValueType = claim.ValueType;
        Issuer = claim.Issuer;
        Value = claim.Value;
    }
}
