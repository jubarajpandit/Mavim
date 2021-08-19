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
    /// ChangeParentCommand
    /// </summary>
    public class ChangeParentCommand : BaseCommand, IChangeParentCommand
    {
        /// <summary>
        /// ChangeParentCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public ChangeParentCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="topicParentId"></param>
        /// <returns></returns>
        public async Task Execute(string topicId, string topicParentId)
        {
            IDcvId topicDcvId = DcvId.FromDcvKey(topicId) ?? throw new BadRequestException($"Supplied topicId format is invalid: {topicId}");
            IDcvId topicParentDcvId = DcvId.FromDcvKey(topicParentId) ?? throw new BadRequestException($"Supplied topicId format is invalid: {topicParentId}");

            if (topicId == topicParentId)
                throw new BadRequestException("The topic id's cannot be the same value");

            IElement topic = _model.ElementRepository.GetElement(topicDcvId);
            if (topic == null)
                throw new RequestNotFoundException("Topic not found");

            IElement newParentTopic = _model.ElementRepository.GetElement(topicParentDcvId);
            if (newParentTopic == null)
                throw new RequestNotFoundException("Parent topic not found");

            // TODO: 32162, This command can be removed when the CanExecute is properly implemented.
            if (!topic.Bizz.CanCut || !newParentTopic.Bizz.CanCutPasteLower(new[] { topic }))
                throw new ForbiddenRequestException("Cannot move topic");

            var moveTopicCommand = _model.Factories.CommandFactory.CreateMoveElementsToParentCommand(new[] { topic }, newParentTopic);

            if (!moveTopicCommand.CanExecute())
                throw new ForbiddenRequestException("Cannot move topic");

            await Task.Run(() => moveTopicCommand.Execute(new ProgressDummy()));
        }
    }
}
