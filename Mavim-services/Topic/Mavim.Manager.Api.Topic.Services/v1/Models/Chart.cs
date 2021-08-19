using Mavim.Manager.Api.Topic.Services.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class Chart : IChart
    {
        /// <summary>
        /// Combined ids of the ChartDcv in string format
        /// </summary>
        public string Dcv { get; set; }
        /// <summary>
        /// Name of the chart
        /// </summary>
        public string Name { get; set; }
    }
}
