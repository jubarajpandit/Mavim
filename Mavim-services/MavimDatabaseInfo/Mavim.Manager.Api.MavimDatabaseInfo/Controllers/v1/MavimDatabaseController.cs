using Mavim.Manager.Api.MavimDatabaseInfo.Services.Interfaces.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.MavimDatabaseInfo.Controllers.v1
{
    [Authorize]
    [Route("/v1/mavimdatabases")]
    public class MavimDatabaseController : ControllerBase
    {
        #region Private Members
        private readonly IMavimDatabaseInfoService _mavimDatabaseInfoService;
        #endregion

        /// <summary>
        /// MavimDatabaseController Constructor
        /// </summary>
        /// <param name="mavimDatabaseService">The mavim database service.</param>
        /// <exception cref="ArgumentNullException">mavimDatabaseService</exception>
        public MavimDatabaseController(IMavimDatabaseInfoService mavimDatabaseService)
        {
            _mavimDatabaseInfoService = mavimDatabaseService ?? throw new ArgumentNullException(nameof(mavimDatabaseService));
        }

        /// <summary>
        /// Retrieves all the MavimDatabases connected to the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IDbConnectionInfo>>> GetMavimDatabases() =>
            Ok(await _mavimDatabaseInfoService.GetMavimDatabases());


        /// <summary>
        /// Retrieves all the MavimDatabases connected to the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{dbId}")]
        public async Task<ActionResult<IDbConnectionInfo>> GetMavimDatabase(Guid dbId) =>
            Ok(await _mavimDatabaseInfoService.GetMavimDatabase(dbId));
    }
}
