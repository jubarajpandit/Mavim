namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums
{
    /// <summary>
    /// ElementType enum - conforms to the the MvmSrv_ELEtpe of the Mavim Manager Server.
    /// </summary>    
    public enum TopicType // TODO: This enum is not a fully blown list of the existing emum in Mavim Manager Server, but the intention is to grow this gradually utilizing what is required at the given point in time. (WI14769)
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// Virtual Element
        /// </summary>
        Virtual,

        /// <summary>
        /// Mavim Element Container
        /// </summary>
        MavimElementContainer,

        /// <summary>
        /// Relationship Categories
        /// </summary>
        RelationCategories,

        /// <summary>
        /// With
        /// </summary>
        WithWhat,

        /// <summary>
        /// Who
        /// </summary>
        Who,

        /// <summary>
        /// Where
        /// </summary>
        Where,

        /// <summary>
        /// When
        /// </summary>
        When,

        /// <summary>
        /// Why
        /// </summary>
        Why,

        /// <summary>
        /// Where to
        /// </summary>
        WhereTo,

        /// <summary>
        /// Recyclebin
        /// </summary>
        RecycleBin,
    }
}
