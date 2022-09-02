using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Saorsa.Pythagoras.Domain;

namespace Saorsa.Pythagoras.RestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigController : ControllerBase
{
    public IOptions<PythagorasOptions> Options { get; }

    public ConfigController(IOptions<PythagorasOptions> pythagorasOptions)
    {
        Options = pythagorasOptions;
    }

    [HttpGet]
    public ActionResult<PythagorasOptions> GetPythagorasConfiguration()
    {
        return Options.Value;
    }
}
