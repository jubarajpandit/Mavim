namespace Mavim.Manager.Api.Connect.Read.Versions.V1.DTO
{
    /// <summary>
    /// Metadata DTO
    /// </summary>
    public record MetaDTO(int ModelVersion, int AggregateId)
    {
        /// <summary>
        /// ModelVersion
        /// </summary>
        public int ModelVersion { get; init; } = ModelVersion;

        /// <summary>
        /// AggregateId
        /// </summary>
        public int AggregateId { get; init; } = AggregateId;
    };
}