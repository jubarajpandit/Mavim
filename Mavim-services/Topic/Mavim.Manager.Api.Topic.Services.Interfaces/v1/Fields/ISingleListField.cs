using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields
{
    public interface ISingleListField : ISingleField<Dictionary<string, string>>
    {
        Dictionary<string, string> Options { get; set; }
    }
}
