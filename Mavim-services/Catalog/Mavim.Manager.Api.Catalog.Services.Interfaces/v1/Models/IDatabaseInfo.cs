using System;

namespace Mavim.Manager.Api.Catalog.Services.Interfaces.v1.Models
{
    public interface IDatabaseInfo
    {
        string DisplayName { get; set; }
        Guid DatabaseID { get; set; }
    }
}
