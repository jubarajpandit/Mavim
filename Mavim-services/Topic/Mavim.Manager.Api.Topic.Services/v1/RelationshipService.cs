using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.v1.Models;
using Mavim.Manager.Api.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1
{
    public class RelationshipService : IRelationshipService
    {
        private IBusiness.IRelationshipBusiness Business { get; }
        private IBusiness.ITopicBusiness TopicBusiness { get; }
        private ILogger<TopicService> Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicService"/> class.
        /// </summary>
        /// <param name="topicBusiness"></param>
        /// <param name="onBehalfOfTokenProvider"></param>
        /// <param name="logger" />
        /// <exception cref="ArgumentNullException"></exception>
        public RelationshipService(IBusiness.IRelationshipBusiness relationBusiness, IBusiness.ITopicBusiness topicBusiness, ILogger<TopicService> logger)
        {
            Business = relationBusiness ?? throw new ArgumentNullException(nameof(relationBusiness));
            TopicBusiness = topicBusiness ?? throw new ArgumentNullException(nameof(topicBusiness));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>Gets the relationship objects for a Mavim database element using the database id and dcv from the Mavim database.</summary>
        /// <param name="dcvId"></param>
        /// <returns />
        public async Task<IEnumerable<IRelationship>> GetRelationships(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IEnumerable<IBusiness.IRelationship> relationships = await Business.GetRelationships(dcvId);

            return relationships.Select(Map);
        }

        /// <summary>
        /// Deletes the relationship.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <param name="relationshipId">The relationship identifier.</param>
        /// <returns />
        public async Task DeleteRelationship(string dcvId, string relationshipId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            if (!DcvUtils.IsValid(relationshipId))
                throw new BadRequestException($"Invalid relationshipId {relationshipId}");

            IBusiness.ITopic topic = await TopicBusiness.GetTopic(dcvId);
            if (topic.IsReadOnly)
                throw new ForbiddenRequestException("Update of topic is forbidden");

            IEnumerable<IBusiness.IRelationship> relationships = await Business.GetRelationships(dcvId);

            if (relationships.FirstOrDefault() == null)
                throw new RequestNotFoundException("Relationship is not found.");

            await Business.DeleteRelationship(dcvId, relationshipId);
        }

        /// <summary>
        /// Save Relationship
        /// </summary>
        /// <param name="patchRelationship" />
        /// <returns />
        public async Task<IRelationship> SaveRelationship(ISaveRelationship patchRelationship)
        {
            if (patchRelationship == null)
            {
                throw new BadRequestException("Invalid body");
            }

            IBusiness.ITopic businessTopic = await TopicBusiness.GetTopic(patchRelationship.FromElementDcv);

            if (businessTopic == null || businessTopic.IsReadOnly)
                throw new ForbiddenRequestException("Update of topic is forbidden");

            IBusiness.IRelationship relationship = await Business.SaveRelationship(Map(patchRelationship));

            return Map(relationship);
        }

        /// <summary>
        /// Map Relationship
        /// </summary>
        /// <param name="relationship" />
        /// <returns />
        private Relationship Map(IBusiness.IRelationship relationship) =>
            new Relationship
            {
                Dcv = relationship.Dcv,
                IsTypeOfTopic = relationship.IsTypeOfTopic,
                Category = relationship.Category,
                CategoryType = Map(relationship.CategoryType),
                RelationshipType = Map(relationship.RelationshipType),
                Icon = relationship.Icon,
                UserInstruction = Map(relationship.UserInstruction),
                DispatchInstructions = Map(relationship.DispatchInstructions),
                Characteristic = Map(relationship.Characteristic),
                WithElement = Map(relationship.WithElement),
                WithElementParent = Map(relationship.WithElementParent)
            };

        private IBusiness.ISaveRelationship Map(ISaveRelationship relationship) =>
            new Business.v1.Models.SaveRelationship
            {
                FromElementDcv = relationship.FromElementDcv,
                ToElementDcv = relationship.ToElementDcv,
                RelationshipType = Map(relationship.RelationshipType)
            };

        private static IEnumerable<ISimpleDispatchInstruction> Map(IEnumerable<IBusiness.ISimpleDispatchInstruction> dispatchInstructions) =>
            dispatchInstructions.Select(dispatchInstruction => new SimpleDispatchInstruction
            {
                TypeName = dispatchInstruction?.TypeName,
                Dcv = dispatchInstruction?.Dcv,
                Name = dispatchInstruction?.Name,
                Icon = dispatchInstruction?.Icon
            });

        private static IRelationshipElement Map(IBusiness.IRelationshipElement topic) =>
            topic == null ? null : new RelationshipElement
            {
                Dcv = topic.Dcv,
                Name = topic.Name,
                Icon = topic.Icon
            };

        private static RelationshipType Map(IBusiness.enums.RelationshipType type) =>
            Enum.TryParse<RelationshipType>(type.ToString(), out var result) ? result : RelationshipType.Unknown;

        private static IBusiness.enums.RelationshipType Map(RelationshipType type) =>
            Enum.TryParse<IBusiness.enums.RelationshipType>(type.ToString(), out var result) ? result : IBusiness.enums.RelationshipType.Unknown;

        private static CategoryType Map(IBusiness.enums.CategoryType type) =>
            Enum.TryParse<CategoryType>(type.ToString(), out var result) ? result : CategoryType.Unknown;

        private static IBusiness.enums.CategoryType Map(CategoryType type) =>
            Enum.TryParse<IBusiness.enums.CategoryType>(type.ToString(), out var result) ? result : IBusiness.enums.CategoryType.Unknown;
    }
}