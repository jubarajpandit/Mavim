using Mavim.Libraries.Features.Enums;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using Mavim.Manager.Api.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;
using Mavim.Manager.Api.Topic.Repository.Features;

namespace Mavim.Manager.Api.Topic.Business.v1
{
    public class TopicBusiness : ITopicBusiness
    {
        private IRepo.Topics.ITopicRepository Repository { get; }
        private ILogger<TopicBusiness> Logger { get; }
        private IFeatureManager FeatureManager { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicBusiness" /> class.
        /// </summary>
        /// <param name="topicRepository">The topic repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="featureManager">The feature manager.</param>
        /// <exception cref="ArgumentNullException">
        /// topicRepository
        /// or
        /// logger
        /// or
        /// featureManager
        /// </exception>
        public TopicBusiness(IRepo.Topics.ITopicRepository topicRepository, ILogger<TopicBusiness> logger, IFeatureManager featureManager)
        {
            Repository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            FeatureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        /// <summary>
        /// Get the root topic from the Mavim database
        /// </summary>
        /// <returns>Root topic</returns>
        public async Task<ITopic> GetRootTopic()
        {
            IRepo.Topics.ITopic root = await Repository.GetRootTopic();
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await FeatureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return customIconFeatureEnabled ? Map(root, customIconFeatureEnabled) : Map(root);
        }

        public async Task<ITopicPath> GetPathToRoot(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IRepo.Topics.ITopicPath topicPath = await Repository.GetPathToRoot(dcvId);

            if (topicPath.Path.Any(x => !IsPublicComponent(topicPath.Data.Find(topic => topic.Dcv.ToString() == x.DcvId))))
                throw new ForbiddenRequestException("Retrieval of topic in path is forbidden");


            if (await FeatureManager.IsEnabledAsync(nameof(TopicBusinessLogicFeature.ExternalReferenceReCount)))
            {
                IEnumerable<Task<IRepo.Topics.ITopic>> tasks = topicPath.Data.Select(RecountExternalRootChildrenAsync);
                await Task.WhenAll(tasks);
            }


            topicPath.Data = topicPath.Data.Where(IsPublicComponent).ToList();            
            return Map(topicPath);
        }

        /// <summary>
        /// Gets the root topic of the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        public async Task<ITopic> GetTopic(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IRepo.Topics.ITopic topic = await Repository.GetTopicByDcv(dcvId);

            if (!IsPublicComponent(topic))
                throw new ForbiddenRequestException("Retrieval of this topic is forbidden");

            if (await FeatureManager.IsEnabledAsync(nameof(TopicBusinessLogicFeature.ExternalReferenceReCount)))
                topic = await RecountExternalRootChildrenAsync(topic);
            
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await FeatureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return customIconFeatureEnabled ? Map(topic, customIconFeatureEnabled) : Map(topic);
        }

        /// <summary>
        /// Gets topics by code of the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="topicCode">The topic code.</param>
        public async Task<IReadOnlyList<ITopic>> GetTopicsByCode(string topicCode)
        {
            if (string.IsNullOrEmpty(topicCode))
                throw new BadRequestException($"Empty topic code.");

            IReadOnlyList<IRepo.Topics.ITopic> topics = await Repository.GetTopicsByCode(topicCode);

            if (await FeatureManager.IsEnabledAsync(nameof(TopicBusinessLogicFeature.ExternalReferenceReCount)))
                await Task.WhenAll(topics.Select(RecountExternalRootChildrenAsync));
            
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await FeatureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return topics.Where(IsPublicComponent).Select(x => customIconFeatureEnabled ? Map(x, customIconFeatureEnabled) : Map(x)).ToList();
        }

        /// <summary>Gets the children of mavim database topic based on the dcv from the Mavim database by establishing the connection with Mavim database using on behalf of access token.</summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns>Collection of child topic</returns>
        public async Task<IEnumerable<ITopic>> GetChildren(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            List<IRepo.Topics.ITopic> children = (await Repository.GetChildrenByDcv(dcvId)).ToList();

            if (await FeatureManager.IsEnabledAsync(nameof(TopicBusinessLogicFeature.ExternalReferenceReCount)))
                await Task.WhenAll(children.Select(RecountExternalRootChildrenAsync));

            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await FeatureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return children.Where(IsPublicComponent).Select(x => customIconFeatureEnabled ? Map(x, customIconFeatureEnabled) : Map(x));
        }

        /// <summary>Gets the siblings of mavim database topic based on the dcv from the Mavim database by establishing the connection with Mavim database using on behalf of access token.</summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns>Collection of child topics</returns>
        public async Task<IEnumerable<ITopic>> GetSiblings(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            List<IRepo.Topics.ITopic> siblings = (await Repository.GetSiblingsByDcv(dcvId)).ToList();


            if (await FeatureManager.IsEnabledAsync(nameof(TopicBusinessLogicFeature.ExternalReferenceReCount)))
                await Task.WhenAll(siblings.Select(RecountExternalRootChildrenAsync));

            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await FeatureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return siblings.Where(IsPublicComponent).Select(x => customIconFeatureEnabled ? Map(x, customIconFeatureEnabled) : Map(x));
        }

        /// <summary>
        /// Gets the relationship objects for a Mavim database element using the database id and dcv from the Mavim database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ITopic>> GetRelationshipCategories()
        {
            IEnumerable<IRepo.Topics.ITopic> relationshipCategories = await Repository.GetRelationshipCategories();

            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await FeatureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return relationshipCategories.Where(IsPublicComponent).Select(x => customIconFeatureEnabled ? Map(x, customIconFeatureEnabled) : Map(x));
        }

        public async Task<ITopic> UpdateTopic(string dcvId, ISaveTopic topic)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            if (topic?.Name == null)
                throw new BadRequestException(nameof(topic));

            IRepo.Topics.ITopic topicToUpdate = await Repository.GetTopicByDcv(dcvId);

            if (IsReadOnlyTopic(topicToUpdate))
                throw new ForbiddenRequestException("Update of topic is forbidden");

            IRepo.Topics.ITopic updatedTopic = await Repository.UpdateTopicName(dcvId, topic.Name);

            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await FeatureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return customIconFeatureEnabled ? Map(updatedTopic, customIconFeatureEnabled) : Map(updatedTopic);
        }

        /// <summary>
        /// Get Topic Custom Icon By TopicCustomIconId
        /// </summary>
        /// <param name="customIconId"></param>
        /// <returns></returns>

        //TODO: Remove Feature Flag => WI: 27606
        public async Task<byte[]> GetTopicCustomIconByCustomIconId(string customIconId)
        {
            if (string.IsNullOrEmpty(customIconId))
                throw new BadRequestException($"Empty Custom Icon Id.");

            return await Repository.GetTopicCustomIconByCustomIconId(customIconId);            
        }

        #region Private Methods
        /// <summary>
        /// Recalculate the number of public children to set the HasChildren boolean.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private async Task<IRepo.Topics.ITopic> RecountExternalRootChildrenAsync(IRepo.Topics.ITopic topic)
        {
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));

            if (!topic.HasChildren || !topic.Type.IsExternalReferencesRoot) return topic;

            IEnumerable<IRepo.Topics.ITopic> children = await Repository.GetChildrenByDcv(topic.Dcv.ToString());
            IEnumerable<IRepo.Topics.ITopic> filteredChildren = children?.Where(IsPublicComponent);

            if (filteredChildren != null && filteredChildren.Any()) return topic;
            topic.HasChildren = false;
            topic.Resource?.Remove(IRepo.enums.TopicResource.SubTopics);

            return topic;
        }

        private bool IsReadOnlyTopic(IRepo.Topics.ITopic topic)
        {
            return topic.Dcv.Ver != 0 ||
                   topic.Type.HasSystemName ||
                   topic.Type.IsImportedVersionsRoot ||
                   topic.Type.IsImportedVersion ||
                   topic.Type.IsCreatedVersionsRoot ||
                   topic.Type.IsCreatedVersion ||
                   topic.Type.IsRecycleBin ||
                   topic.Type.IsRelationshipsCategoriesRoot ||
                   topic.Type.IsExternalReferencesRoot ||
                   topic.Type.IsObjectsRoot ||
                   topic.IsInRecycleBin;
        }

        private readonly Func<IRepo.Topics.ITopic, bool> IsInternalComponent = topic =>
                    topic.IsInternal || topic.ElementType == IRepo.enums.ElementType.Object;

        private bool IsPublicComponent(IRepo.Topics.ITopic topic)
        {
            return topic != null && (!IsInternalComponent(topic) || topic.IsChart);
        }

        /// <summary>
        /// Maps the topic repository object to the topic data transfer object.
        /// </summary>
        /// <param name="topic">The element.</param>
        /// <returns></returns>
        private ITopic Map(IRepo.Topics.ITopic topic)
        {
            return new Models.Topic
            {
                Dcv = topic.Dcv.ToString(),
                Parent = topic.Parent,
                Name = topic.Name,
                Code = topic.Code,
                Icon = topic.Icon,
                IsChart = topic.IsChart,
                IsReadOnly = IsReadOnlyTopic(topic),
                HasChildren = topic.HasChildren,
                CanDelete = topic.CanDelete,
                CanCreateChildTopic = topic.CanCreateChildTopic,
                CanCreateTopicAfter = topic.CanCreateTopicAfter,
                IsInRecycleBin = topic.IsInRecycleBin,
                OrderNumber = topic.OrderNumber,
                ElementType = Map(topic.ElementType),
                Resource = topic.Resource?.Select(Map).ToList()
            };
        }

        //TODO: Remove Feature Flag => WI: 27606
        private ITopic Map(IRepo.Topics.ITopic topic, bool customIconFeatureEnabled)
        {
            var result = Map(topic);            
            result.CustomIconId = customIconFeatureEnabled ? topic.CustomIconId : null;
            return result;
        }

        private ITopicPath Map(IRepo.Topics.ITopicPath topicPath)
        {
           return new Models.TopicPath { Path = topicPath?.Path?.Select(Map).ToList(), Data = topicPath?.Data?.Select(Map).ToList() };
        }

        private IPathItem Map(IRepo.Topics.IPathItem pathItem)
        {
            return new Models.PathItem { Order = pathItem.Order, DcvId = pathItem.DcvId };
        }

        private TopicResource Map(IRepo.enums.TopicResource resources) => resources switch
        {
            IRepo.enums.TopicResource.Chart => TopicResource.Chart,
            IRepo.enums.TopicResource.Description => TopicResource.Description,
            IRepo.enums.TopicResource.Fields => TopicResource.Fields,
            IRepo.enums.TopicResource.Relations => TopicResource.Relations,
            IRepo.enums.TopicResource.SubTopics => TopicResource.SubTopics,
            _ => throw new ArgumentOutOfRangeException(nameof(resources))
        };

        /// <summary>
        /// Maps the repository category type to the Business category type equivalent, or unknown if type is not found.
        /// </summary>
        /// <param name="type">The repository category type</param>
        /// <returns></returns>
        private ElementType Map(IRepo.enums.ElementType type) =>
            Enum.TryParse<ElementType>(type.ToString(), out var result) ? result : ElementType.Unknown;
        #endregion
    }
}
