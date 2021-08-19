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
    /// MoveToTopCommand
    /// </summary>
    public class MoveToTopCommand : BaseCommand, IMoveToTopCommand
    {
        /// <summary>
        /// MoveToTopCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public MoveToTopCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

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

            var moveToTopCommand = _model.Factories.CommandFactory.CreateMoveElementToFirstPositionInBranchCommand(topic);
            if (!moveToTopCommand.CanExecute())
                throw new ForbiddenRequestException("Cannot move given topic to the top of the branch");

            await Task.Run(() => moveToTopCommand.Execute(new ProgressDummy()));
        }
    }
}
