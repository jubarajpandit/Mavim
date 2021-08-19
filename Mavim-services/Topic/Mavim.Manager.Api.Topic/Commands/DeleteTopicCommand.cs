using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Commands.Interfaces;
using Mavim.Manager.Model;
using Mavim.Manager.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Commands
{
    /// <summary>
    /// DeleteTopicCommand
    /// </summary>
    public class DeleteTopicCommand : BaseCommand, IDeleteTopicCommand
    {
        /// <summary>
        /// DeleteTopicCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public DeleteTopicCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task Execute(string topicId)
        {
            IElement topic = GetTopicById(topicId) ?? throw new BadRequestException($"Topic to delete not found: {topicId}");
            IMavimDatabaseModelCommand command = _model.Factories.CommandFactory.CreateDeleteElementsCommand(new List<IElement> { topic }.ToArray(), false);

            if (!command.CanExecute())
                throw new ForbiddenRequestException($"Unable to delete topic: {topicId}");

            await Task.Run(() => command.Execute(new ProgressDummy()));
        }

        private IElement GetTopicById(string topicId)
        {
            IDcvId topicDcvId = DcvId.FromDcvKey(topicId);
            if (topicDcvId == null)
                throw new BadRequestException($"Supplied topicId format is invalid: {topicId}");

            return _model.ElementRepository.GetElement(topicDcvId);
        }
    }
}
