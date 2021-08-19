using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields
{
    public interface ISingleRelationshipListField : ISingleField<Dictionary<string, IRelationshipElement>>
    {
        Dictionary<string, IRelationshipElement> Options { get; set; }
    }
}
