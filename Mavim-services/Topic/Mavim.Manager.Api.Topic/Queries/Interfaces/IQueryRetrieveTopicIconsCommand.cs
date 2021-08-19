using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Queries.Interfaces
{
    /// <summary>
    /// IQueryRetrieveTopicIconsCommand
    /// </summary>
    public interface IQueryRetrieveTopicIconsCommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        Task<IDictionary<string, string>> Execute(string elementType);
    }
}
