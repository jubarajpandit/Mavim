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
    /// MoveLevelUpCommand
    /// </summary>
    public class MoveLevelUpCommand : BaseCommand, IMoveLevelUpCommand
    {
        /// <summary>
        /// MoveLevelUpCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public MoveLevelUpCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

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

            var moveLevelUpCommand = _model.Factories.CommandFactory.CreateMoveElementOneLevelUpCommand(topic);
            if (!moveLevelUpCommand.CanExecute())
                throw new ForbiddenRequestException("Cannot move given topic one level up");

            await Task.Run(() => moveLevelUpCommand.Execute(new ProgressDummy()));
        }
    }
}
