using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Controllers.v1
{
    /// <summary>
    /// SystemObjectsController
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/{dbId}/{dataLanguage}/systemobjects/")]
    public class SystemObjectsController : ControllerBase
    {
        private ITopicService Service { get; set; }
        /// <summary>
        /// SystemObjectsController Constructor
        /// </summary>
        /// <param name="service">The service.</param>
        public SystemObjectsController(ITopicService service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Gets the topics reflecting the relationship categories
        /// </summary>
        /// <remarks>Get the relationship categories of the topic</remarks>
        /// <param name="dbId">The Mavim database identifier.</param>
        /// <param name="dataLanguage">The Topic language.</param>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ITopic>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("topic/relationshipcategories")]
        public async Task<ActionResult<IEnumerable<ITopic>>> GetRelationCategories(
            Guid dbId,
            DataLanguages dataLanguage)
        {
            return Ok(await Service.GetRelationshipCategories());
        }
    }
}
