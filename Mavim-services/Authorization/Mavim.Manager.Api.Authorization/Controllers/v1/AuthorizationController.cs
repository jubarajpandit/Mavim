using Mavim.Manager.Api.Authorization.Services.Interfaces.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAuthorizationService = Mavim.Manager.Api.Authorization.Services.Interfaces.v1.IAuthorizationService;
using Service = Mavim.Manager.Api.Authorization.Services.v1.Model;

namespace Mavim.Manager.Api.Authorization.Controllers.v1
{
    [Authorize]
    [Route("/v1/authorize")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(IAuthorizationService authorizationService, ILogger<AuthorizationController> logger)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all users role based on tentant
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IUser>> GetAllUsers() =>
            Ok(await _authorizationService.GetUsers());

        /// <summary>
        /// Add users with role
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<IUser>>> AddUsers([FromBody] List<User> users)
        {
            if (ModelState.IsValid)
            {
                if (users == null || !users.Any())
                    return BadRequest("Empty user list");

                return Ok(await _authorizationService.AddUsers(users.Select(Map)));
            }
            return ReturnModelStateError();
        }

        [HttpPatch]
        [Route("user/{id}")]
        public async Task<ActionResult> PatchUser(Guid id, [FromBody] EmailPatchBody body)
        {
            if (ModelState.IsValid)
            {
                await _authorizationService.EditUserRole(id, body.Role);
                return Ok();
            }
            return ReturnModelStateError();
        }

        [HttpDelete]
        [Route("user/{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            if (ModelState.IsValid)
            {
                await _authorizationService.DeleteUser(id);
                return Ok();
            }
            return ReturnModelStateError();
        }

        /// <summary>
        /// Receive user role based on email
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<IUser>> GetUser() =>
            Ok(await _authorizationService.GetUser());

        private static IUser Map(User user) => new Service.User()
        {
            Email = user.Email,
            Role = user.Role,
        };

        private ActionResult ReturnModelStateError() =>
            ModelState.Keys.FirstOrDefault()?.ToLower()?.Replace("[0].", "") switch
            {
                "id" => BadRequest("Invalid UserID"),
                "role" => BadRequest("Invalid role"),
                "email" => BadRequest("Invalid email"),
                _ => BadRequest("Invalid user object")
            };

    }

}
