using Mavim.Manager.Api.FeatureFlag.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.FeatureFlag.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/v1/[controller]")]
    public class FeatureFlagController : ControllerBase
    {
        public FeatureFlagController() { }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetFeatureFlag(
            [FromServices] IMediator mediator) => Ok(await mediator.Send(new GetActiveFeatureFlags.Query()));
    }
}
