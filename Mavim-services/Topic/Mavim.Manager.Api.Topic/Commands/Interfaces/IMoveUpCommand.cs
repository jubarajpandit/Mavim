using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Commands.Interfaces
{
    /// <summary>
    /// IMoveUpCommand
    /// </summary>
    public interface IMoveUpCommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task Execute(string topicId);
    }
}
