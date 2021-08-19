namespace Mavim.Manager.Api.Connect.Read.Versions.V1.DTO
{
    /// <summary>
    /// Update User DTO
    /// </summary>
    public record UpdateGroupDTO(string Name, string Description, int ModelVersion, int AggregateId) : MetaDTO(ModelVersion, AggregateId)
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; init; } = Name;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; init; } = Description;
    };
}