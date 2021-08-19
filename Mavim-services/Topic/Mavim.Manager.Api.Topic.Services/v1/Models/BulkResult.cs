using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class BulkResult<T> : IBulkResult<T>
    {
        public List<T> Succeeded { get; set; }
        public List<IFailed<T>> Failed { get; set; }
    }
}