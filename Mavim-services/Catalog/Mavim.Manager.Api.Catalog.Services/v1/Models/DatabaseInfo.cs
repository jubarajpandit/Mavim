using Mavim.Manager.Api.Catalog.Services.Interfaces.v1.Models;
using System;

namespace Mavim.Manager.Api.Catalog.Services.v1.Models
{
    public class DatabaseInfo : IDatabaseInfo
    {
        public string DisplayName { get; set; }
        public Guid DatabaseID { get; set; }
    }
}
