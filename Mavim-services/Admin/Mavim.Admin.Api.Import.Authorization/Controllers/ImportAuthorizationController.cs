using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.Authorization.Repository.Interfaces.v1.Interface;
using Mavim.Manager.Api.Authorization.Repository.v1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mavim.Admin.Api.Import.Authorization.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/v1/admin/import")]
    public class ImportAuthorizationController : ControllerBase
    {
        private readonly IAuthorizationRepository _repository;
        public ImportAuthorizationController(IAuthorizationRepository repo)
        {
            _repository = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpPost]
        [Route("authorize")]
        public async Task<ActionResult<List<IUser>>> Import(List<Model.User> userList)
        {
            IEnumerable<IUser> users = userList.Select(Map);
            return Ok(await _repository.ImportByUsersObjectId(users));
        }

        private IUser Map(Model.User user) => new User
            {
                Id = user.ObjectId.ToString(),
                TenantId = user.TenantId,
                Role = UserRole.Contributor
            };
    }
}
