using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.v1.Models.Fields.Abstract
{
    public abstract class MultiField<T> : Field, IMultiField<T>
    {
        public IEnumerable<T> Data { get; set; }
    }
}
