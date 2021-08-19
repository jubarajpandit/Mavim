using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.v1.Models;
using Mavim.Manager.Api.Topic.Services.v1.Mappers;
using Mavim.Manager.Api.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Service = Mavim.Manager.Api.Topic.Services.v1.Models;
using Microsoft.FeatureManagement;
using Mavim.Manager.Api.Topic.Repository.Features;

namespace Mavim.Manager.Api.Topic.Services.v1
{
    public class TopicService : ITopicService
    {
        private IBusiness.ITopicBusiness _business { get; }
        private ILogger<TopicService> _logger { get; }
        private readonly IFeatureManager _featureManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicService"/> class.
        /// </summary>
        /// <param name="topicBusiness"></param>
        /// <param name="onBehalfOfTokenProvider"></param>
        /// <param name="logger" />
        /// <exception cref="ArgumentNullException"></exception>
        public TopicService(IBusiness.ITopicBusiness topicBusiness, ILogger<TopicService> logger, IFeatureManager featureManager)
        {
            _business = topicBusiness ?? throw new ArgumentNullException(nameof(topicBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        /// <summary>
        /// Get the root topic from the Mavim database
        /// </summary>
        /// <returns>Root topic</returns>
        public async Task<ITopic> GetRootTopic(Guid dbId)
        {

            IBusiness.ITopic root = await _business.GetRootTopic();
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));

            //TODO: Remove Feature Flag => WI: 27606
            return customIconFeatureEnabled ? TopicMapper.MapTopicWithResource(root, customIconFeatureEnabled) : TopicMapper.MapTopicWithResource(root);
        }

        public async Task<ITopicPath> GetPathToRoot(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IBusiness.ITopicPath topicPath = await _business.GetPathToRoot(dcvId);

            return TopicMapper.MapTopicPath(topicPath);
        }

        /// <summary>
        /// Gets the root topic of the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        public async Task<ITopic> GetTopic(Guid dbId, string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IBusiness.ITopic topic = await _business.GetTopic(dcvId);
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));

            //TODO: Remove Feature Flag => WI: 27606
            return customIconFeatureEnabled ? TopicMapper.MapTopicWithResource(topic, customIconFeatureEnabled) : TopicMapper.MapTopicWithResource(topic);
        }

        /// <summary>
        /// Gets the topics by code of the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="code">The topic code.</param>
        public async Task<IReadOnlyList<ITopic>> GetTopicsByCode(string topicCode)
        {
            IEnumerable<IBusiness.ITopic> topics = await _business.GetTopicsByCode(topicCode);

            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return topics.Select(x => customIconFeatureEnabled ? TopicMapper.Map(x, customIconFeatureEnabled) : TopicMapper.Map(x)).ToList();
        }

        /// <summary>Gets the children of mavim database topic based on the dcv from the Mavim database by establishing the connection with Mavim database using on behalf of access token.</summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns>Collection of child topic</returns>
        public async Task<IEnumerable<ITopic>> GetChildren(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IEnumerable<IBusiness.ITopic> children = await _business.GetChildren(dcvId);
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return children.Select(x => customIconFeatureEnabled ? TopicMapper.Map(x, customIconFeatureEnabled) : TopicMapper.Map(x));
        }

        /// <summary>Gets the siblings of mavim database topic based on the dcv from the Mavim database by establishing the connection with Mavim database using on behalf of access token.</summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns>Collection of child topics</returns>
        public async Task<IEnumerable<ITopic>> GetSiblings(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IEnumerable<IBusiness.ITopic> siblings = await _business.GetSiblings(dcvId);

            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return siblings.Select(x => customIconFeatureEnabled ? TopicMapper.Map(x, customIconFeatureEnabled) : TopicMapper.Map(x));
        }

        /// <summary>
        /// Gets the relationship objects for a Mavim database element using the database id and dcv from the Mavim database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ITopic>> GetRelationshipCategories()
        {
            IEnumerable<IBusiness.ITopic> relationshipCategories = await _business.GetRelationshipCategories();

            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return relationshipCategories.Select(x => customIconFeatureEnabled ? TopicMapper.Map(x, customIconFeatureEnabled) : TopicMapper.Map(x));
        }

        public async Task<ITopic> UpdateTopic(string dcvId, ISaveTopic topic)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            if (topic?.Name == null)
                throw new BadRequestException(nameof(topic));

            IBusiness.ITopic updatedTopic = await _business.UpdateTopic(dcvId, TopicMapper.MapSaveTopic(topic));

            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return customIconFeatureEnabled ? TopicMapper.Map(updatedTopic, customIconFeatureEnabled) : TopicMapper.Map(updatedTopic);
        }
    }
}
