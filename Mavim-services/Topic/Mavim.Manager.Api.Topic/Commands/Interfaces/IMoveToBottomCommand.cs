using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Commands.Interfaces
{
    /// <summary>
    /// IMoveToBottomCommand
    /// </summary>
    public interface IMoveToBottomCommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task Execute(string topicId);
    }
}
