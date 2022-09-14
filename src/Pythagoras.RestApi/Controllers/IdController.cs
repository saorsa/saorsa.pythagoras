using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saorsa.Pythagoras.Domain.Auth;
using Saorsa.Pythagoras.Domain.Model;
using Saorsa.Pythagoras.RestApi.Model;

namespace Saorsa.Pythagoras.RestApi.Controllers;

[ApiController]
[Authorize, Route("[controller]")]
public class IdController : ControllerBase
{
    public IPythagorasSessionManager SessionManager { get; }

    public IdController(IPythagorasSessionManager sessionManager)
    {
        SessionManager = sessionManager;
    }

    [HttpGet]
    public ActionResult<DTOIdentityContext> WhoAmI()
    {
        var user = SessionManager.GetLoggedInUser()!;
        var claims = (this.User.Identity as ClaimsIdentity)?
            .Claims.Select(c => new IdentityClaim(c)) ?? Array.Empty<IdentityClaim>();

        return Ok(new DTOIdentityContext(user, claims));
    }
}
