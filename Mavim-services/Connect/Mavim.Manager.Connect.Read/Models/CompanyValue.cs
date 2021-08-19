using Mavim.Manager.Connect.Read.Models.Interfaces;
using System;

namespace Mavim.Manager.Connect.Read.Models
{
    public record CompanyValue(Guid Id, string Name, string Domain, Guid TenantId) : ICompanyValue;
}
