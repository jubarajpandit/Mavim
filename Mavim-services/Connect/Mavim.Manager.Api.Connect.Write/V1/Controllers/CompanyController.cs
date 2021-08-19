using Mavim.Manager.Api.Connect.Write.Extensions;
using Mavim.Manager.Api.Connect.Write.Featureflags;
using Mavim.Manager.Api.Connect.Write.V1.DTO;
using Mavim.Manager.Connect.Write.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Connect.Write.V1.Controllers
{
    /// <summary>
    /// Company Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = AuthorizationHandlerExtension.InternalSchemaName, Roles = "connectwriteserver")]
    [ApiController]
    [ApiVersion("1.0")]
    [FeatureGate(Features.ConnectWrite)]
    [Route("internal/v{version:apiVersion}/[controller]")]
    public class CompanyController : ControllerBase
    {
        /// <summary>
        /// Add Company Endpoint
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="company"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CreateDto>> AddCompany([FromServices] IMediator mediator, AddCompanyDto company, CancellationToken token)
        {
            var (name, domain, tenantId) = company;
            return ModelState.IsValid
                ? Ok(new CreateDto(await mediator.Send(new AddCompany.Command(name, domain, tenantId), token)))
                : BadRequest(ModelState);
        }

        /// <summary> 
        /// Undo Eventsourcing based on date
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="company"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("undo")]
        public async Task<ActionResult> UndoEventSourcing([FromServices] IMediator mediator, UndoEventSourcingDto company, CancellationToken token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await mediator.Send(new UndoEventSourcing.Command(company.StartDate), token);
            return NoContent();
        }

        /// <summary>
        /// Resend Eventsourcing based on entityId and aggregateId
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="company"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("resend")]
        public async Task<ActionResult> ResendEventSourcing([FromServices] IMediator mediator, ResendEventSourcingDto company, CancellationToken token)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (aggregateId, entityId) = company;
            await mediator.Send(new ResendEventSourcing.Command(aggregateId, entityId), token);
            return NoContent();
        }

    }
}