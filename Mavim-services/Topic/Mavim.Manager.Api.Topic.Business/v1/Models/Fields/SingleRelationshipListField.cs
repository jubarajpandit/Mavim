using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Business.v1.Models.Fields.Abstract;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Business.v1.Models.Fields
{
    public class SingleRelationshipListField : SingleField<Dictionary<string, IRelationshipElement>>, ISingleRelationshipListField
    {
        public Dictionary<string, IRelationshipElement> Options { get; set; }
    }
}
