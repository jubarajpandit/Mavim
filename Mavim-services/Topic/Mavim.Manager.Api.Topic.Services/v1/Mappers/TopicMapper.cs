using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Service = Mavim.Manager.Api.Topic.Services.v1.Models;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mavim.Manager.Api.Topic.Services.v1.Models;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;

namespace Mavim.Manager.Api.Topic.Services.v1.Mappers
{
    public static class TopicMapper
    {
        #region Private Methods
        /// <summary>
        /// Maps the topic Business object to the topic data transfer object.
        /// </summary>
        /// <param name="topic">The element.</param>
        /// <returns></returns>
        public static ITopic Map(IBusiness.ITopic topic)
        {
            return new Service.Topic
            {
                Dcv = topic.Dcv,
                Parent = topic.Parent,
                HasChildren = topic.HasChildren,
                Name = topic.Name,
                Code = topic.Code,
                Icon = topic.Icon,
                OrderNumber = topic.OrderNumber,
                TypeCategory = MapToElementType(topic.ElementType),
                IsInRecycleBin = topic.IsInRecycleBin,
                Business = new TopicBusiness()
                {
                    IsReadOnly = topic.IsReadOnly,
                    CanDelete = topic.CanDelete,
                    CanCreateChildTopic = topic.CanCreateChildTopic,
                    CanCreateTopicAfter = topic.CanCreateTopicAfter
                }
            };
        }

        //TODO: Remove Feature Flag => WI: 27606
        public static ITopic Map(IBusiness.ITopic topic, bool customIconFeatureEnabled)
        {
            var result = Map(topic);            
            result.CustomIconId = customIconFeatureEnabled ? topic.CustomIconId : null;
            return result;
        }

        /// <summary>
        /// Maps the topic Business object with the resources
        /// </summary>
        /// <param name="topic">The element.</param>
        /// <returns></returns>
        public static ITopic MapTopicWithResource(IBusiness.ITopic topic)
        {
            ITopic topicWithResource = Map(topic);
            topicWithResource.Resources = topic.Resource?.Select(Map).ToList();
            return topicWithResource;
        }

        //TODO: Remove Feature Flag => WI: 27606
        public static ITopic MapTopicWithResource(IBusiness.ITopic topic, bool customIconFeatureEnabled)
        {
            ITopic topicWithResource = customIconFeatureEnabled ? Map(topic, customIconFeatureEnabled) : Map(topic);
            topicWithResource.Resources = topic.Resource?.Select(Map).ToList();
            return topicWithResource;
        }

        private static TopicResource Map(IBusiness.enums.TopicResource resource) => resource switch
        {
            IBusiness.enums.TopicResource.Chart => TopicResource.Chart,
            IBusiness.enums.TopicResource.Description => TopicResource.Description,
            IBusiness.enums.TopicResource.Fields => TopicResource.Fields,
            IBusiness.enums.TopicResource.Relations => TopicResource.Relations,
            IBusiness.enums.TopicResource.SubTopics => TopicResource.SubTopics,
            _ => throw new ArgumentOutOfRangeException(nameof(resource)),
        };

        public static IBusiness.ISaveTopic MapSaveTopic(ISaveTopic topic)
        {
            return new Business.v1.Models.SaveTopic
            {
                Name = topic.Name
            };
        }

        public static ITopicPath MapTopicPath(IBusiness.ITopicPath topicPath)
        {
            if (topicPath?.Path == null || topicPath?.Data == null) return null;
            return new TopicPath { Path = topicPath?.Path?.Select(MapPathItem).ToList(), Data = topicPath?.Data?.Select(Map).ToList() };
        }

        private static IPathItem MapPathItem(IBusiness.IPathItem pathItem)
        {
            return new PathItem { Order = pathItem.Order, DcvId = pathItem.DcvId };
        }

        /// <summary>
        /// Maps the Business category type to the service category type equivalent, or unknown if type is not found.
        /// </summary>
        /// <param name="type">The Business category type</param>
        /// <returns></returns>
        public static TopicType MapToElementType(IBusiness.enums.ElementType type) =>
            Enum.TryParse<TopicType>(type.ToString(), out var result) ? result : TopicType.Unknown;
        #endregion
    }
}
