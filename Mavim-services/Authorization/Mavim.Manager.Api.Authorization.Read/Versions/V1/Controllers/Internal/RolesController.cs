using System;
using System.Threading.Tasks;
using Mavim.Manager.Api.Authorization.Read.Extensions;
using Mavim.Manager.Api.Authorization.Read.Featureflags;
using Mavim.Manager.Api.Authorization.Read.Versions.V1.DTO;
using Mavim.Manager.Authorization.Read.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace Mavim.Manager.Api.Authorization.Read.Versions.V1.Controllers.Internal
{
    /// <summary>
    /// Internal GroupsController
    /// </summary>
    [Authorize(AuthenticationSchemes = AuthorizationHandlerExtension.InternalSchemaName, Roles = "authreadserver")]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.AuthorizationRead)]
    [Route("internal/v{version:apiVersion}/[controller]")]
    public class RolesController : ControllerBase
    {
        /// <summary>
        /// Add Role Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddRoleV1([FromServices] IMediator mediator, [FromBody] AddRole role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (id, name, groups, topicPermissions, companyId, aggregateId) = role;

            await mediator.Send(new AddRoleCommand.Command(id, name, groups, topicPermissions, companyId, aggregateId));
            return NoContent();
        }

        /// <summary>
        /// Update Role Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="roleId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPatch("{roleId}")]
        public async Task<ActionResult> UpdateRoleV1([FromServices] IMediator mediator, [FromRoute] Guid roleId, [FromBody] UpdateRole role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (name, companyId, aggregateId) = role;

            await mediator.Send(new UpdateRoleCommand.Command(roleId, name, companyId, aggregateId));
            return NoContent();
        }

        /// <summary>
        /// Add Groups To Role Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="roleId"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        [HttpPatch("{roleId}/users")]
        public async Task<ActionResult> AddUsersToRoleV1([FromServices] IMediator mediator, [FromRoute] Guid roleId, [FromBody] RoleGroups groups)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (groupIds, companyId, aggregateId) = groups;

            await mediator.Send(new AddGroupToRoleCommand.Command(roleId, groupIds, companyId, aggregateId));
            return NoContent();
        }

        /// <summary>
        /// Delete Groups From Role Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="roleId"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        [HttpDelete("{roleId}/users")]
        public async Task<ActionResult> DeleteGroupsFromRoleV1([FromServices] IMediator mediator, [FromRoute] Guid roleId, [FromBody] RoleGroups groups)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (groupIds, companyId, aggregateId) = groups;

            await mediator.Send(new DeleteGroupFromRoleCommand.Command(roleId, groupIds, companyId, aggregateId));
            return NoContent();
        }

        /// <summary>
        /// Disable Role Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="roleId"></param>
        /// <param name="disabledRole"></param>
        /// <returns></returns>
        [HttpPatch("{roleId}/disabled")]
        public async Task<ActionResult> DisableRoleV1([FromServices] IMediator mediator, [FromRoute] Guid roleId, [FromBody] DisabledRole disabledRole)
        {
            if (!ModelState.IsValid) return BadRequest(disabledRole);

            var (disabled, companyId, aggregateId) = disabledRole;

            if (disabled)
                await mediator.Send(new DisableRoleCommand.Command(roleId, companyId, aggregateId));
            else
                await mediator.Send(new EnableRoleCommand.Command(roleId, companyId, aggregateId));


            return NoContent();
        }
    }
}
