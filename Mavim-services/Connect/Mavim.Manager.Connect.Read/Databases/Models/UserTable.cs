using System;

namespace Mavim.Manager.Connect.Read.Databases.Models
{
    public record UserTable(Guid Id, string Value, int ModelVersion, int AggregateId, Guid CompanyId, bool Disabled, DateTime LastUpdated) :
        ReadModel(Id, Value, ModelVersion, AggregateId, CompanyId, Disabled, LastUpdated);
}
