using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields
{
    public interface IMultiRelationListField : IMultiField<IRelationshipElement>
    {
        IEnumerable<IRelationshipElement> Options { get; set; }
    }
}