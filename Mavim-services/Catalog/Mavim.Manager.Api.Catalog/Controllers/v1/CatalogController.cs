using Mavim.Manager.Api.Catalog.Services.Interfaces.v1;
using Mavim.Manager.Api.Catalog.Services.Interfaces.v1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Catalog.Controllers.v1
{
    [Authorize]
    [Route("/v1/mavimdatabases")]
    public class CatalogController : ControllerBase
    {
        #region Private Members
        private readonly ICatalogService _catalogService;
        #endregion

        /// <summary>
        /// MavimDatabaseController Constructor
        /// </summary>
        /// <param name="mavimDatabaseService">The mavim database service.</param>
        /// <exception cref="ArgumentNullException">mavimDatabaseService</exception>
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        }

        /// <summary>
        /// Retrieves all the MavimDatabases connected to the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IDatabaseInfo>>> GetMavimDatabases() =>
            Ok(await _catalogService.GetMavimDatabases());


        /// <summary>
        /// Retrieves all the MavimDatabases connected to the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{dbId}")]
        public async Task<ActionResult<IDatabaseInfo>> GetMavimDatabase(Guid dbId) =>
            Ok(await _catalogService.GetMavimDatabase(dbId));
    }
}
