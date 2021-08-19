namespace Mavim.Manager.Api.Topic.Business.Interfaces.v1
{
    public interface IChart
    {
        /// <summary>
        /// Combined ids of the ChartDcv in string format
        /// </summary>
        string Dcv { get; set; }
        /// <summary>
        /// Name of the chart
        /// </summary>
        string Name { get; set; }
    }
}
