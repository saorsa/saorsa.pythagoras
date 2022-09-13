using AutoMapper;
using Saorsa.Pythagoras.Domain.Auth;

namespace Saorsa.Pythagoras.Domain.Business;

public interface IPythagorasBusinessService
{
    IPythagorasSessionManager SessionManager { get; }
    
    IMapper Mapper { get; }
}
