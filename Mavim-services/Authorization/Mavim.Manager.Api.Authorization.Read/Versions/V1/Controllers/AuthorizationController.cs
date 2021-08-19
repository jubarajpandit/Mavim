using System.Collections.Generic;
using System.Threading.Tasks;
using Mavim.Manager.Api.Authorization.Read.Featureflags;
using Mavim.Manager.Authorization.Read.Models;
using Mavim.Manager.Authorization.Read.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace Mavim.Manager.Api.Authorization.Read.Versions.V1.Controllers
{
    /// <summary>
    /// Roles Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.AuthorizationRead)]
    [Route("/v{version:apiVersion}/[controller]")]
    public class RolesController : ControllerBase
    {
        /// <summary>
        /// Get All Roles from my Company Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Role>>> GetRolesV1([FromServices] IMediator mediator)
            => Ok(await mediator.Send(new GetRoles.Query()));


        /// <summary>
        /// Get All Roles based on the groups i'm in
        /// </summary>
        /// <param name="mediator"></param>
        /// <returns></returns>
        [HttpGet("me")]
        public async Task<ActionResult<IReadOnlyList<Role>>> GetMyRolesV1([FromServices] IMediator mediator)
            => Ok(await mediator.Send(new GetMyRoles.Query()));
    }
}
