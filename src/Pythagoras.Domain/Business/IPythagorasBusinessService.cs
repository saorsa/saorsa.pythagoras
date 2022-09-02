using AutoMapper;
using Saorsa.Pythagoras.Domain.Auth;

namespace Saorsa.Pythagoras.Domain.Business;

public interface IPythagorasBusinessService
{
    IPythagorasIdentityProvider IdentityProvider { get; }
    
    IMapper Mapper { get; }
}
