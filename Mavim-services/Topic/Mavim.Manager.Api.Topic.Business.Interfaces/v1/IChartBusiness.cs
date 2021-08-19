﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Business.Interfaces.v1
{
    public interface IChartBusiness
    {


        /// <summary>
        /// Gets the Charts of topic by DCV identifier.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<IChart>> GetTopicCharts(string dcvId);
    }
}