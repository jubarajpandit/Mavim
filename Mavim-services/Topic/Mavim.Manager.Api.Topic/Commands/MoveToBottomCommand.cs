using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Commands.Interfaces;
using Mavim.Manager.Model;
using Mavim.Manager.Utils;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Commands
{
    /// <summary>
    /// MoveToBottomCommand
    /// </summary>
    public class MoveToBottomCommand : BaseCommand, IMoveToBottomCommand
    {
        /// <summary>
        /// MoveToBottomCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public MoveToBottomCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task Execute(string topicId)
        {
            IDcvId topicDcvId = DcvId.FromDcvKey(topicId);

            IElement topic = _model.ElementRepository.GetElement(topicDcvId);
            if (topic == null)
                throw new BadRequestException("Topic to move not found");

            var moveToBottomCommand = _model.Factories.CommandFactory.CreateMoveElementToLastPositionInBranchCommand(topic);
            if (!moveToBottomCommand.CanExecute())
                throw new ForbiddenRequestException("Cannot move topic to the bottom of the branch");

            await Task.Run(() => moveToBottomCommand.Execute(new ProgressDummy()));
        }
    }
}
