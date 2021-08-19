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
    /// MoveLevelDownCommand
    /// </summary>
    public class MoveLevelDownCommand : BaseCommand, IMoveLevelDownCommand
    {
        /// <summary>
        /// MoveLevelDownCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public MoveLevelDownCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

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

            var moveLevelDownCommand = _model.Factories.CommandFactory.CreateMoveElementOneLevelDownCommand(topic);
            if (!moveLevelDownCommand.CanExecute())
                throw new ForbiddenRequestException("Cannot move given topic one level down");

            await Task.Run(() => moveLevelDownCommand.Execute(new ProgressDummy()));
        }
    }
}
