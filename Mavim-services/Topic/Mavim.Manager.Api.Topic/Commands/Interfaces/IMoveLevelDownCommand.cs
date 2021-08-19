using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Commands.Interfaces
{
    /// <summary>
    /// IMoveLevelDownCommand
    /// </summary>
    public interface IMoveLevelDownCommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Task Execute(string topicId);
    }
}
