using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saorsa.Pythagoras.Domain.Auth;

namespace Saorsa.Pythagoras.RestApi.Controllers;

[ApiController]
[Authorize, Route("[controller]")]
public class IdController : ControllerBase
{
    public IPythagorasIdentityProvider IdentityProvider { get; }

    public IdController(IPythagorasIdentityProvider identityProvider)
    {
        IdentityProvider = identityProvider;
    }

    [HttpGet]
    public ActionResult WhoAmI()
    {
        var user = IdentityProvider.GetLoggedInUser();
        return Ok(new
        {
            User = user,
            User2 = (this.User.Identity as ClaimsIdentity)?.Claims.Select(c => new
            {
                c.Type,
                c.Value,
                c.ValueType,
                c.Issuer
            })
        });
    }
}
