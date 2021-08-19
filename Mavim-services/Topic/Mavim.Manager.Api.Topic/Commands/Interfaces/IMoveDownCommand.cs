using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Commands.Interfaces
{
    /// <summary>
    /// IMoveDownCommand
    /// </summary>
    public interface IMoveDownCommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task Execute(string topicId);
    }
}
