using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using Mavim.Manager.Api.Topic.Repository.v1.Fields.Base;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.v1.Fields
{
    public class SingleRelationshipListField : SingleField<Dictionary<string, IRelationshipElement>>, ISingleRelationshipListField
    {
        public Dictionary<string, IRelationshipElement> Options { get; set; }
    }
}
