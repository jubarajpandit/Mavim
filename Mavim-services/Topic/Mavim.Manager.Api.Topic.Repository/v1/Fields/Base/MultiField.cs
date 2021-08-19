using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Topic.Repository.v1.Fields.Base
{
    public abstract class MultiField<T> : Field, IMultiField<T>
    {
        /// <summary>
        /// FieldValue
        /// </summary>
        public IEnumerable<T> FieldValues { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        public IEnumerable<T> DefaultValues { get; set; }
    }
}
