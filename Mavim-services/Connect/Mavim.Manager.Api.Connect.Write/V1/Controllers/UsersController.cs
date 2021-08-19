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
    /// User Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.ConnectWrite)]
    [Route("/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Add User Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> AddUser([FromServices] IMediator mediator, AddUserDto user,
            CancellationToken token)
        {
            return ModelState.IsValid
                ? Ok(new CreateDto(await mediator.Send(new AddUser.Command(user.Email), token)))
                : BadRequest(ModelState);
        }

        /// <summary>
        /// Delete User Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser([FromServices] IMediator mediator, [RequiredGuid] Guid userId,
            CancellationToken token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await mediator.Send(new DeleteUser.Command(userId), token);
            return NoContent();
        }
    }
}