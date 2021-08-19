namespace Mavim.Manager.Api.Utils.AzAppConfiguration
{
    public class WopiSettings
    {
        /// <summary>
        /// Gets or sets the Database connectionString for Wopi database.
        /// In this case this is same as the Topic database.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the discovery URL.
        /// Discovery URL is used by the Wopi middleware to obtain information like about Proofkey: oldvalue, oldModulus etc.
        /// For more information on the values that can be obtained from the discovery url take a look at the dogfood discovery url below.
        /// https://ffc-onenote.officeapps.live.com/hosting/discovery
        /// </summary>
        /// <value>
        /// The discovery URL.
        /// </value>
        public string DiscoveryUrl { get; set; }

        /// <summary>
        /// Gets or sets the proof key information cache expiration in minutes.
        /// Proofkeys should have default expiration of 20 mins as per the WOPI documentation and WOPI timestamp is used to determine the expiration.
        /// However, the default 20 mins can be changed using this expiration property.
        /// </summary>
        /// <value>
        /// The proof key information cache expiration in minutes.
        /// </value>
        public int ProofKeyInfoCacheExpirationInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the request URL origin.
        /// This URL is used while deconstructing the proofkey and this url should match the original https ur of WOPI Host used by Office 365 Client to make call.
        /// The URL used by the backend hosted by AKS is usually different than the one exposed outside for Office 365 Client and hence this has to be a configuration value that cab be further used by the AKS.
        /// </summary>
        /// <value>
        /// The request URL origin.
        /// </value>
        public string RequestUrlOrigin { get; set; }

        /// <summary>
        /// Gets or sets the host edit URL.
        /// In case of BUSINESS_USER flow (which is mandatory for us to implement for commercial purposes),
        /// This URL is utilized by the WOPI Client to redirect the user back to the WOPI HOST EDIT page after successfully loggin in to the O365 subscription.
        /// For BUSINESS_USER O365 client automatically detects if the user is logged in or not and redirects to the login page if not logged in.
        /// Once logged in, the user is redirected to the host edit page using this property.
        /// </summary>
        /// <value>
        /// The host edit URL.
        /// </value>
        public string HostEditUrl { get; set; }

        /// <summary>
        /// Gets or sets the wopi file lock database connection string.
        /// This property is used to connect to the FileLock database which is used for maintaining the version or
        /// file metadata information which is used by the WOPI Client to validate before rendering the document to the end user.
        /// </summary>
        /// <value>
        /// The wopi file lock database connection string.
        /// </value>
        public string WopiFileLockDbConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the supported wopi query parameter values.
        /// </summary>
        /// <value>
        /// The supported wopi query parameter values.
        /// </value>
        public string SupportedWopiQueryParameterValues { get; set; }
    }
}
