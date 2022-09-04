using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Saorsa.Pythagoras.Domain;
using Saorsa.Pythagoras.Domain.Configuration;

namespace Saorsa.Pythagoras.RestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigController : ControllerBase
{
    public IOptions<PythagorasConfiguration> Options { get; }

    public ConfigController(IOptions<PythagorasConfiguration> pythagorasOptions)
    {
        Options = pythagorasOptions;
    }

    [HttpGet]
    public ActionResult<PythagorasConfiguration> GetPythagorasConfiguration()
    {
        return Options.Value;
    }
}
