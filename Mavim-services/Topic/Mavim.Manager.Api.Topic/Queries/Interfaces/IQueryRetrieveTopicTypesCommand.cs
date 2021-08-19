using Mavim.Manager.Api.Topic.v1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Queries.Interfaces
{
    /// <summary>
    /// IQueryRetrieveTopicTypesCommand
    /// </summary>
    public interface IQueryRetrieveTopicTypesCommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task<IDictionary<string, ElementTypeInfo>> Execute(string topicId);
    }
}
