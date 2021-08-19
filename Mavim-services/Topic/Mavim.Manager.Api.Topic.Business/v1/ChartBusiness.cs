using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.v1.Models;
using Mavim.Manager.Api.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Business.v1
{
    public class ChartBusiness : IChartBusiness
    {
        private IRepo.RelationShips.IRelationshipsRepository Repository { get; }
        private ILogger<ChartBusiness> Logger { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TopicBusiness"/> class.
        /// </summary>
        /// <param name="topicRepository"></param>
        /// <param name="onBehalfOfTokenProvider"></param>
        /// <param name="logger" />
        /// <exception cref="ArgumentNullException"></exception>
        public ChartBusiness(IRepo.RelationShips.IRelationshipsRepository relationRepository, ILogger<ChartBusiness> logger)
        {
            Repository = relationRepository ?? throw new ArgumentNullException(nameof(relationRepository));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>Gets the Chart objects for a Mavim database element using the database id and dcv from the Mavim database.</summary>
        /// <param name="dcvId"></param>
        /// <returns />
        public async Task<IEnumerable<IChart>> GetTopicCharts(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IEnumerable<IRepo.RelationShips.IRelationship> relationships = await Repository.GetRelationships(dcvId);

            return relationships.Where(r => IsChartAndPublic(r)).Select(Map);
        }

        #region Private Methods

        private bool IsChartAndPublic(IRepo.RelationShips.IRelationship relation)
        {
            return !relation.IsDestroyed && relation.CategoryType == IRepo.enums.CategoryType.Chart && relation.WithElement.IsPublic;
        }

        /// <summary>
        /// Map Relationship to IChart
        /// </summary>
        /// <param name="relationship" />
        /// <returns />
        private IChart Map(IRepo.RelationShips.IRelationship relationship) =>
            new Chart
            {
                Dcv = relationship.WithElement.Dcv,
                Name = relationship.WithElement.Name
            };
    }
    #endregion
}