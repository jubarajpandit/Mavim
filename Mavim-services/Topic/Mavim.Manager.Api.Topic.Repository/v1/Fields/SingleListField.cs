using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.v1.Fields.Base;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.v1.Fields
{
    public class SingleListField : SingleField<Dictionary<string, string>>, ISingleListField
    {
        public Dictionary<string, string> Options { get; set; }
    }
}
