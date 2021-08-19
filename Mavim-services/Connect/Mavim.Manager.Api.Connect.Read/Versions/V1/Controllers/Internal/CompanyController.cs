using Mavim.Manager.Api.Connect.Read.Extensions;
using Mavim.Manager.Api.Connect.Read.Featureflags;
using Mavim.Manager.Api.Connect.Read.Versions.V1.DTO;
using Mavim.Manager.Connect.Read.Commands;
using Mavim.Manager.Connect.Read.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Connect.Read.Versions.V1.Controllers.Internal
{
    /// <summary>
    /// Internal CompanyController
    /// </summary>
    [Authorize(AuthenticationSchemes = AuthorizationHandlerExtension.InternalSchemaName, Roles = "connectreadserver")]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.ConnectRead)]
    [Route("internal/v{version:apiVersion}/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly int _version = 1;
        /// <summary>
        /// Add Company Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddCompanyV1([FromServices] IMediator mediator, [FromBody] AddCompanyDTO company)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (company.ModelVersion != _version) return UnprocessableEntity(new { Error = string.Format(Logging.INCORRECT_MODELVERSION, company.ModelVersion, _version) });

            await mediator.Send(new AddCompanyCommand.Command(company.Id, company.Name, company.Domain, company.TenantId, company.ModelVersion, company.AggregateId));
            return NoContent();
        }
    }
}
