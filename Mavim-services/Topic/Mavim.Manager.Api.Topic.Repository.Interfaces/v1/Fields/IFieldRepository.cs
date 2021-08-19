using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields
{
    public interface IFieldRepository
    {
        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<IField>> GetFields(string fieldSetId);

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <param name="fieldSetId">The field set identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns></returns>
        Task<IField> GetField(string topicId, string fieldSetId, string fieldId);

        /// <summary>
        /// Updates the field value.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <param name="fieldSetId">The field set identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns></returns>
        Task<IField> UpdateFieldValue(IField field, string dcvId, string fieldSetId, string fieldId);
    }
}
