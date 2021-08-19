using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields.Abstract;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.v1.Models.Fields
{
    public class SingleListField : SingleField<Dictionary<string, string>>, ISingleListField
    {
        public Dictionary<string, string> Options { get; set; }
    }
}
