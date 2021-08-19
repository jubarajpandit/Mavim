using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Commands.Interfaces
{
    /// <summary>
    /// ICreateChildTopicCommand
    /// </summary>
    public interface ICreateChildTopicCommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="topicName"></param>
        /// <param name="topicType"></param>
        /// <param name="topicIcon"></param>
        /// <returns></returns>
        Task<ITopic> Execute(string referenceId, string topicName, string topicType, string topicIcon);
    }
}
