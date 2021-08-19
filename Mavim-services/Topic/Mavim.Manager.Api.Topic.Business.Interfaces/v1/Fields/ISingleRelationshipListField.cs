using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields
{
    public interface ISingleRelationshipListField : ISingleField<Dictionary<string, IRelationshipElement>>
    {
        Dictionary<string, IRelationshipElement> Options { get; set; }
    }
}
