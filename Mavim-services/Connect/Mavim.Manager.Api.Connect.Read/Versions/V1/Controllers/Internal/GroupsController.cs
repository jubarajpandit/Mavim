using Mavim.Manager.Api.Connect.Read.Extensions;
using Mavim.Manager.Api.Connect.Read.Featureflags;
using Mavim.Manager.Api.Connect.Read.Versions.V1.DTO;
using Mavim.Manager.Connect.Read.Commands;
using Mavim.Manager.Connect.Read.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Connect.Read.Versions.V1.Controllers.Internal
{

    /// <summary>
    /// Internal GroupsController
    /// </summary>
    [Authorize(AuthenticationSchemes = AuthorizationHandlerExtension.InternalSchemaName, Roles = "connectreadserver")]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.ConnectRead)]
    [Route("internal/v{version:apiVersion}/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly int _version = 1;

        /// <summary>
        /// Add Group Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddGroupV1([FromServices] IMediator mediator, [FromBody] AddGroupDTO group)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (group.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, group.ModelVersion, _version) });

            await mediator.Send(new AddGroupCommand.Command(group.Id, group.Name, group.Description, group.CompanyId, group.ModelVersion, group.AggregateId));
            return NoContent();
        }

        /// <summary>
        /// Update Group Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPatch("{groupId}")]
        public async Task<ActionResult> UpdateGroupV1([FromServices] IMediator mediator, [FromRoute] Guid groupId, [FromBody] UpdateGroupDTO group)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (group.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, group.ModelVersion, _version) });

            await mediator.Send(new UpdateGroupCommand.Command(groupId, group.Name, group.Description, group.ModelVersion, group.AggregateId));
            return NoContent();
        }

        /// <summary>
        /// Add Users To Group Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [HttpPatch("{groupId}/users")]
        public async Task<ActionResult> AddUsersToGroupV1([FromServices] IMediator mediator, [FromRoute] Guid groupId, [FromBody] UserIdsDTO userIds)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (userIds.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, userIds.ModelVersion, _version) });

            await mediator.Send(new AddUsersToGroupCommand.Command(groupId, userIds.Ids, userIds.ModelVersion, userIds.AggregateId));
            return NoContent();
        }

        /// <summary>
        /// Delete Users From Group Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [HttpDelete("{groupId}/users")]
        public async Task<ActionResult> DeleteUsersFromGroupV1([FromServices] IMediator mediator, [FromRoute] Guid groupId, [FromBody] UserIdsDTO userIds)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (userIds.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, userIds.ModelVersion, _version) });

            await mediator.Send(new DeleteUsersFromGroupCommand.Command(groupId, userIds.Ids, userIds.ModelVersion, userIds.AggregateId));
            return NoContent();
        }

        /// <summary>
        /// Disable Group Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        [HttpDelete("{groupId}/disable")]
        public async Task<ActionResult> DisableGroupV1([FromServices] IMediator mediator, [FromRoute] Guid groupId, [FromBody] MetaDTO metadata)
        {
            if (!ModelState.IsValid) return BadRequest(metadata);
            if (metadata.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, metadata.ModelVersion, _version) });

            await mediator.Send(new DisableGroupCommand.Command(groupId, metadata.ModelVersion, metadata.AggregateId));
            return NoContent();
        }

        /// <summary>
        /// Enable Group Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="groupId"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        [HttpPatch("{groupId}/enable")]
        public async Task<ActionResult> EnableGroupV1([FromServices] IMediator mediator, [FromRoute] Guid groupId, [FromBody] MetaDTO metadata)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (metadata.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, metadata.ModelVersion, _version) });

            await mediator.Send(new EnableGroupCommand.Command(groupId, metadata.ModelVersion, metadata.AggregateId));
            return NoContent();
        }
    }
}
