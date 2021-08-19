using Mavim.Manager.Api.Connect.Write.Featureflags;
using Mavim.Manager.Api.Connect.Write.V1.DTO;
using Mavim.Manager.Api.Connect.Write.Validators;
using Mavim.Manager.Connect.Write.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Connect.Write.V1.Controllers
{
    /// <summary>
    /// Group Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.ConnectWrite)]
    [Route("/v{version:apiVersion}/[controller]")]
    public class GroupsController : ControllerBase
    {
        /// <summary>
        /// Add Group Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="group"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CreateDto>> AddGroup([FromServices] IMediator mediator, AddGroupDto group, CancellationToken token)
        {
            var (name, description) = group;
            return ModelState.IsValid
                ? Ok(new CreateDto(await mediator.Send(new AddGroup.Command(name, description), token)))
                : BadRequest(ModelState);
        }

        /// <summary>
        /// Update Group Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <param name="group"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPatch("{groupId}")]
        public async Task<ActionResult> UpdateGroup([FromServices] IMediator mediator, [RequiredGuid] Guid groupId,
            UpdateGroupDto group, CancellationToken token)
        {
            var (name, description) = group;
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await mediator.Send(new UpdateGroup.Command(groupId, name, description), token);
            return NoContent();
        }

        /// <summary>
        /// Delete Group Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete("{groupId}")]
        public async Task<ActionResult> DeleteGroup([FromServices] IMediator mediator, [RequiredGuid] Guid groupId, CancellationToken token)
        {
            await mediator.Send(new DeleteGroup.Command(groupId), token);
            return NoContent();
        }

        /// <summary>
        /// Add Users To UserGroup
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <param name="group"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPatch("{groupId}/users")]
        public async Task<ActionResult> UpdateGroupUser([FromServices] IMediator mediator, [RequiredGuid] Guid groupId,
            UpdateUserGroupDto group, CancellationToken token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await mediator.Send(new UpdateUserGroup.Command(groupId, group.UserIds), token);
            return NoContent();
        }

        /// <summary>
        /// Remove Users From UserGroup
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <param name="group"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete("{groupId}/users")]
        public async Task<ActionResult> DeleteGroupUser([FromServices] IMediator mediator, [RequiredGuid] Guid groupId,
            DeleteUserGroupDto group, CancellationToken token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await mediator.Send(new DeleteUserGroup.Command(groupId, group.UserIds), token);
            return NoContent();
        }
    }
}