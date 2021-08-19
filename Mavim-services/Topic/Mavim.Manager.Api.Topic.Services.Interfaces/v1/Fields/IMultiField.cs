using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields
{
    public interface IMultiField<T> : IField
    {
        /// <summary>
        /// Collection with data objects
        /// </summary>
        IEnumerable<T> Data { get; set; }

    }
}
