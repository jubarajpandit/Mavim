using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Commands.Interfaces
{
    /// <summary>
    /// IChangeParentCommand
    /// </summary>
    public interface IChangeParentCommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="topicParentId"></param>
        /// <returns></returns>
        Task Execute(string topicId, string topicParentId);
    }
}
