using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.v1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.v1.Mappers.Interfaces
{
    /// <summary>
    /// IFieldMapper
    /// </summary>
    public interface IFieldMapper
    {
        /// <summary>
        /// MapFields
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<List<IField>> MapFields(string topicId, Fields fields);

        /// <summary>
        /// MapField
        /// </summary>
        /// <param name="field"></param>
        /// <param name="topicId"></param>
        /// <param name="fieldSetId"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        IField MapField(Field field, string topicId, string fieldSetId, string fieldId);

        /// <summary>
        /// MapField
        /// </summary>
        /// <param name="field"></param>
        /// <param name="topicId"></param>
        /// <param name="fieldSetId"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        IField MapField(IField field, string topicId, string fieldSetId, string fieldId);
    }
}