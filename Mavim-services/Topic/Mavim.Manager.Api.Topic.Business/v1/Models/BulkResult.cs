using Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Business.v1.Models
{
    public class BulkResult<T> : IBulkResult<T>
    {
        public List<T> Succeeded { get; set; }
        public List<IFailed<T>> Failed { get; set; }
    }
}