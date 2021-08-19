using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Commands.Interfaces;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Model;
using Mavim.Manager.Server;
using Mavim.Manager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service = Mavim.Manager.Api.Topic.Services.v1.Models;

namespace Mavim.Manager.Api.Topic.Commands
{
    /// <summary>
    /// CreateChildTopicCommand
    /// </summary>
    public class CreateChildTopicCommand : BaseCommand, ICreateChildTopicCommand
    {
        /// <summary>
        /// CreateChildTopicCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public CreateChildTopicCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="topicName"></param>
        /// <param name="topicType"></param>
        /// <param name="topicIcon"></param>
        /// <returns></returns>
        public async Task<ITopic> Execute(string referenceId, string topicName, string topicType, string topicIcon)
        {
            IElement parent = GetTopicById(referenceId) ?? throw new BadRequestException($"Supplied reference topic to create child topic not found: {referenceId}");
            IElementType elementType = GetTypeFromTopicByString(parent, topicType) ?? throw new BadRequestException($"Invalid topic type supplied for this topic: {topicType}");
            IIconType iconType = GetIconTypeFromElementTypeByString(elementType, topicIcon) ?? throw new BadRequestException($"Invalid topic icon supplied for this topic: {topicType}");
            IElement createdElement = CreateChildTopic(parent, elementType, iconType, topicName) ?? throw new NullReferenceException($"Creation of child topic with parent {referenceId} failed");

            return await Task.Run(() => { return Map(createdElement); });
        }

        private IElement GetTopicById(string referenceId)
        {
            IDcvId referenceDcvId = DcvId.FromDcvKey(referenceId);
            if (referenceDcvId == null)
                throw new BadRequestException($"Supplied topicId format is invalid: {referenceId}");

            return _model.ElementRepository.GetElement(referenceDcvId);
        }

        private IElementType GetTypeFromTopicByString(IElement parent, string topicType)
        {
            if (!Enum.TryParse(topicType, true, out ELEBuffer.MvmSrv_ELEtpe elementTypeEnum))
                throw new BadRequestException($"Unknown topic type supplied: {topicType}");

            IEnumerable<IElementType> types = _model.Queries.GetAllowedChildElementTypes(parent);
            return types.FirstOrDefault(x => x?.Type == elementTypeEnum);
        }

        private IIconType GetIconTypeFromElementTypeByString(IElementType elementType, string topicIcon)
        {
            IEnumerable<IIconType> elementTypeIcons = _model.Queries.GetAllowedIconTypes(elementType);
            return elementTypeIcons.FirstOrDefault(x => x?.IconResourceID.ToString("G").ToLower() == topicIcon?.ToLower());
        }

        private IElement CreateChildTopic(IElement parent, IElementType elementType, IIconType iconType, string topicName)
        {
            var createTopicAfterCommand = _model.Factories.CommandFactory.CreateCreateElementLowerCommand(parent, topicName);
            if (!createTopicAfterCommand.CanExecute())
                throw new ForbiddenRequestException("Cannot create child topic in given topic");

            IElement createdElement = (IElement)createTopicAfterCommand.Execute(new ProgressDummy())
                ?? throw new NullReferenceException($"Creation of topic after {parent?.DcvID?.ToString()} failed");

            createdElement.Type = elementType;
            createdElement.IconType = iconType;
            createdElement.Name = topicName;

            return createdElement;
        }

        private ITopic Map(IElement topic)
        {
            return new Service.Topic
            {
                Dcv = topic.DcvID?.ToString(),
                Parent = topic.Parent?.DcvID?.ToString(),
                HasChildren = topic.HasChildren,
                Name = topic.Name,
                Icon = topic.Visual?.IconResourceID.ToString("G"),
                OrderNumber = topic.OrderNumber,
                TypeCategory = TopicType.Unknown,
                IsInRecycleBin = topic.IsDeleted,
                Business = new Service.TopicBusiness()
                {
                    IsReadOnly = topic.Type?.HasSystemName ?? false,
                    CanDelete = topic.Bizz?.CanDelete ?? false,
                    CanCreateChildTopic = topic.Bizz?.CanCreateNewElementLower ?? false,
                    CanCreateTopicAfter = topic.Bizz?.CanCreateNewElementUnder ?? false
                }
            };
        }
    }
}
