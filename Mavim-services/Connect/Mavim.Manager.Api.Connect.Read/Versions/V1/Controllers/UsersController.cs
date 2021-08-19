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
    /// UsersController
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.ConnectRead)]
    [Route("/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Get All Users From My Company Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<IUserValue>>> GetCompanyUsersV1([FromServices] IMediator mediator)
            => Ok(await mediator.Send(new GetCompanyUsers.Query()));

        /// <summary>
        /// Get Me Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <returns></returns>
        [HttpGet("me")]
        public async Task<ActionResult<IUserValue>> GetMeV1([FromServices] IMediator mediator)
            => Ok(await mediator.Send(new GetMe.Query()));

        /// <summary>
        /// Get My Company Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <returns></returns>
        [HttpGet("me/company")]
        public async Task<ActionResult<ICompanyValue>> GetMyCompanyV1([FromServices] IMediator mediator)
            => Ok(await mediator.Send(new GetMyCompany.Query()));

        /// <summary>
        /// Get My Groups Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <returns></returns>
        [HttpGet("me/groups")]
        public async Task<ActionResult<IReadOnlyList<IGroupValue>>> GetMyGroupsV1([FromServices] IMediator mediator)
            => Ok(await mediator.Send(new GetMyGroups.Query()));

        /// <summary>
        /// Get My Group By Id Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet("me/groups/{groupId}")]
        public async Task<ActionResult<IGroupValue>> GetMyGroupByIdV1([FromServices] IMediator mediator, Guid groupId)
            => Ok(await mediator.Send(new GetMyGroupById.Query(groupId)));
    }
}
