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
    /// Internal UsersController
    /// </summary>
    [Authorize(AuthenticationSchemes = AuthorizationHandlerExtension.InternalSchemaName, Roles = "connectreadserver")]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.ConnectRead)]
    [Route("internal/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly int _version = 1;

        /// <summary>
        /// Add User Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddUserV1([FromServices] IMediator mediator, [FromBody] AddUserDTO user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (user.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, user.ModelVersion, _version) });

            await mediator.Send(new AddUserCommand.Command(user.Id, user.Email, user.CompanyId, user.ModelVersion, user.AggregateId));
            return NoContent();
        }

        /// <summary>
        /// Disable User Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="userId"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        [HttpDelete("{userId}/disable")]
        public async Task<ActionResult> DisableUserV1([FromServices] IMediator mediator, [FromRoute] Guid userId, [FromBody] MetaDTO metadata)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (metadata.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, metadata.ModelVersion, _version) });

            await mediator.Send(new DisableUserCommand.Command(userId, metadata.ModelVersion, metadata.AggregateId));
            return NoContent();
        }

        /// <summary>
        /// Enable User Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="userId"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        [HttpPatch("{userId}/enable")]
        public async Task<ActionResult> EnableUserV1([FromServices] IMediator mediator, [FromRoute] Guid userId, [FromBody] MetaDTO metadata)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (metadata.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, metadata.ModelVersion, _version) });

            await mediator.Send(new EnableUserCommand.Command(userId, metadata.ModelVersion, metadata.AggregateId));
            return NoContent();
        }
    }
}
