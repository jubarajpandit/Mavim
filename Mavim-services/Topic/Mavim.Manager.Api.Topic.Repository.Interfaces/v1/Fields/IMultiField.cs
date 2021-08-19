using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields
{
    public interface IMultiField<T> : IField
    {
        /// <summary>
        /// FieldValue
        /// </summary>
        IEnumerable<T> FieldValues { get; set; }
    }
}
