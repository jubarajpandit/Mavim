using Mavim.Manager.Api.Connect.Read.Featureflags;
using Mavim.Manager.Connect.Read.Models.Interfaces;
using Mavim.Manager.Connect.Read.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Connect.Read.Versions.V1.Controllers
{

    /// <summary>
    /// GroupsController
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.ConnectRead)]
    [Route("v{version:apiVersion}/[controller]")]
    public class GroupsController : ControllerBase
    {
        /// <summary>
        /// Get Groups From My Company Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<IGroupValue>>> GetCompanyGroupsV1([FromServices] IMediator mediator)
            => Ok(await mediator.Send(new GetCompanyGroups.Query()));

        /// <summary>
        /// Get Group by Id Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet("{groupId}")]
        public async Task<ActionResult<IGroupValue>> GetGroupByIdV1([FromServices] IMediator mediator, [FromRoute] Guid groupId)
            => Ok(await mediator.Send(new GetGroupById.Query(groupId)));
    }
}
