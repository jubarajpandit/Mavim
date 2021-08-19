using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields
{
    public interface IBulkResult<T>
    {
        List<T> Succeeded { get; set; }
        List<IFailed<T>> Failed { get; set; }
    }
}