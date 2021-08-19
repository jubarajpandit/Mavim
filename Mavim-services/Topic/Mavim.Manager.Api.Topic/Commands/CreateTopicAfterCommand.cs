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
    /// CreateTopicAfterCommand
    /// </summary>
    public class CreateTopicAfterCommand : BaseCommand, ICreateTopicAfterCommand
    {
        /// <summary>
        /// CreateTopicAfterCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public CreateTopicAfterCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

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
            IElement referenceTopic = GetTopicById(referenceId) ?? throw new BadRequestException($"Supplied reference topic to create topic below not found: {referenceId}");
            IElementType elementType = GetTypeFromTopicByString(referenceTopic, topicType) ?? throw new BadRequestException($"Invalid topic type supplied for this topic: {topicType}");
            IIconType iconType = GetIconTypeFromElementTypeByString(elementType, topicIcon) ?? throw new BadRequestException($"Invalid topic icon supplied for this topic: {topicIcon}");
            IElement createdTopic = CreateTopicAfter(referenceTopic, elementType, iconType, topicName) ?? throw new NullReferenceException($"Creation of topic after {referenceId} failed");


            return await Task.Run(() => { return Map(createdTopic); });
        }

        private IElement GetTopicById(string referenceId)
        {
            IDcvId referenceDcvId = DcvId.FromDcvKey(referenceId);
            if (referenceDcvId == null)
                throw new BadRequestException($"Supplied topicId format is invalid: {referenceId}");

            return _model.ElementRepository.GetElement(referenceDcvId);
        }

        private IElementType GetTypeFromTopicByString(IElement referenceTopic, string topicType)
        {
            if (!Enum.TryParse(topicType, true, out ELEBuffer.MvmSrv_ELEtpe elementTypeEnum))
                throw new BadRequestException($"Unknown topic type supplied: {topicType}");

            IEnumerable<IElementType> types = _model.Queries.GetAllowedChildElementTypes(referenceTopic.Parent);
            return types.FirstOrDefault(x => x?.Type == elementTypeEnum);
        }

        private IIconType GetIconTypeFromElementTypeByString(IElementType elementType, string topicIcon)
        {
            IEnumerable<IIconType> elementTypeIcons = _model.Queries.GetAllowedIconTypes(elementType);
            return elementTypeIcons.FirstOrDefault(x => x?.IconResourceID.ToString("G").ToLower() == topicIcon?.ToLower());
        }

        private IElement CreateTopicAfter(IElement referenceTopic, IElementType elementType, IIconType iconType, string topicName)
        {
            var createTopicAfterCommand = _model.Factories.CommandFactory.CreateCreateElementAfterCommand(referenceTopic);
            if (!createTopicAfterCommand.CanExecute())
                throw new ForbiddenRequestException("Cannot create topic below reference topic");

            IElement createdElement = (IElement)createTopicAfterCommand.Execute(new ProgressDummy())
                ?? throw new NullReferenceException($"Creation of topic after {referenceTopic?.DcvID?.ToString()} failed");

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
                Parent = topic.Parent?.DcvID.ToString(),
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
