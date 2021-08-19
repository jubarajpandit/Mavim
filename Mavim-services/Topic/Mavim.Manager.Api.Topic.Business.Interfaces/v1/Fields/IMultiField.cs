using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields
{
    public interface IMultiField<T> : IField
    {
        /// <summary>
        /// Data
        /// </summary>
        IEnumerable<T> Data { get; set; }

    }
}
