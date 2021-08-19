using System;

namespace Mavim.Libraries.Authorization.Models
{
    public class AzureAdAppConnectionString
    {
        private readonly Guid tenantId;
        private readonly Guid appId;
        private readonly string appSecret;

        public AzureAdAppConnectionString(Guid tenantId, Guid appId, string appSecret)
        {
            this.tenantId = tenantId;
            this.appId = appId;
            this.appSecret = appSecret;
        }

        public static implicit operator string(AzureAdAppConnectionString azAdApp) =>
            $"RunAs=App;AppId={azAdApp.appId};TenantId={azAdApp.tenantId};AppKey={azAdApp.appSecret}";

        public override string ToString() => $"RunAs=App;AppId={appId};TenantId={tenantId};AppKey={appSecret}";
    }
}
