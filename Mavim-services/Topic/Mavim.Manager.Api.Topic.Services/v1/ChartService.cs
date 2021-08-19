using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.v1.Models;
using Mavim.Manager.Api.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1
{
    public class ChartService : IChartService
    {
        private IBusiness.IChartBusiness Business { get; }
        private ILogger<TopicService> Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicService"/> class.
        /// </summary>
        /// <param name="topicBusiness"></param>
        /// <param name="onBehalfOfTokenProvider"></param>
        /// <param name="logger" />
        /// <exception cref="ArgumentNullException"></exception>
        public ChartService(IBusiness.IChartBusiness chartBusiness, ILogger<TopicService> logger)
        {
            Business = chartBusiness ?? throw new ArgumentNullException(nameof(chartBusiness));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>Gets the Chart objects for a Mavim database element using the database id and dcv from the Mavim database.</summary>
        /// <param name="dcvId"></param>
        /// <returns />
        public async Task<IEnumerable<IChart>> GetTopicCharts(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IEnumerable<IBusiness.IChart> charts = await Business.GetTopicCharts(dcvId);

            return charts.Select(Map);
        }

        private static IChart Map(IBusiness.IChart chart) =>
            chart == null ? null : new Chart
            {
                Dcv = chart.Dcv,
                Name = chart.Name
            };
    }
}