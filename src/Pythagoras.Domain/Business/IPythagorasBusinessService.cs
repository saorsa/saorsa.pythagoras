using AutoMapper;

namespace Saorsa.Pythagoras.Domain.Business;

public interface IPythagorasBusinessService
{
    IIdentityProvider IdentityProvider { get; }
    
    IMapper Mapper { get; }
}
