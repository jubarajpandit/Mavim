using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Commands.Interfaces
{
    /// <summary>
    /// IDeleteTopicCommand
    /// </summary>
    public interface IDeleteTopicCommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task Execute(string topicId);
    }
}
