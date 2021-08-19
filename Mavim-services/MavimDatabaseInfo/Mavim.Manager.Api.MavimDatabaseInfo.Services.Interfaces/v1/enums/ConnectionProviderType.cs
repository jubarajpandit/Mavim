namespace Mavim.Manager.Api.Catalog.Interfaces.v1.enums
{
    /// <summary>
    /// ElementType enum - conforms to the the MvmSrv_ELEtpe of the Mavim Manager Server.
    /// </summary>    
    public enum ConnectionProviderType
    {
        /// <summary>
        /// Connection provided to User
        /// </summary>
        User = 0,

        /// <summary>
        /// Connection provided to Domain
        /// </summary>
        Domain = 1,

        /// <summary>
        /// Connection provided to IdentityProvider
        /// </summary>
        IdentityProvider = 2
    }
}
