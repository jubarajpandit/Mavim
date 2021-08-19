

using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Repository.v1.Fields.Base
{
    public abstract class SingleField<T> : Field, ISingleField<T>
    {
        /// <summary>
        /// FieldValue
        /// </summary>
        public T FieldValue { get; set; }
    }
}
