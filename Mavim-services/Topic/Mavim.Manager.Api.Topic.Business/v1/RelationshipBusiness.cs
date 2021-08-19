using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Business.v1.Models;
using Mavim.Manager.Api.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Business.v1
{
    public class RelationshipBusiness : IRelationshipBusiness
    {
        private IRepo.RelationShips.IRelationshipsRepository Repository { get; }
        private ILogger<RelationshipBusiness> Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicBusiness"/> class.
        /// </summary>
        /// <param name="topicRepository"></param>
        /// <param name="onBehalfOfTokenProvider"></param>
        /// <param name="logger" />
        /// <exception cref="ArgumentNullException"></exception>
        public RelationshipBusiness(IRepo.RelationShips.IRelationshipsRepository relationRepository, ILogger<RelationshipBusiness> logger)
        {
            Repository = relationRepository ?? throw new ArgumentNullException(nameof(relationRepository));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>Gets the relationship objects for a Mavim database element using the database id and dcv from the Mavim database.</summary>
        /// <param name="dcvId"></param>
        /// <returns />
        public async Task<IEnumerable<IRelationship>> GetRelationships(string dcvId)
        {
            if (!DcvUtils.IsValid(dcvId))
                throw new BadRequestException($"Invalid DcvID {dcvId}");

            IEnumerable<IRepo.RelationShips.IRelationship> relationships = await Repository.GetRelationships(dcvId);

            return relationships.Where(relation => IsValidRelation(relation)).Select(Map);
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

            IEnumerable<IRepo.RelationShips.IRelationship> relationships = await Repository.GetRelationships(dcvId);

            if (relationships.FirstOrDefault() == null)
                throw new RequestNotFoundException("Relationship is not found.");

            bool deleteResult = await Repository.DeleteRelationship(dcvId, relationshipId);

            if (!deleteResult)
                throw new InvalidOperationException($"Cannot delete relationship");
        }

        /// <summary>
        /// Save Relationship
        /// </summary>
        /// <param name="patchRelationship" />
        /// <returns />
        public async Task<IRelationship> SaveRelationship(ISaveRelationship patchRelationship)
        {
            IRepo.RelationShips.IRelationship relationship = await Repository.CreateRelationship(
                patchRelationship.FromElementDcv,
                patchRelationship.ToElementDcv,
                Map(patchRelationship.RelationshipType));

            return Map(relationship);
        }
        #region Private Methods
        /// <summary>
        /// IsDestroyed is a removed relation
        /// IsAttributeRelation (Also called blue relation) is a way to view a supporting field relation
        /// Checks if the relation category type is not unknown
        /// </summary>
        /// <param name="relation"></param>
        private bool IsValidRelation(IRepo.RelationShips.IRelationship relation)
        {
            return !relation.IsDestroyed && !relation.IsAttributeRelation && relation.CategoryType != IRepo.enums.CategoryType.Unknown && relation.CategoryType != IRepo.enums.CategoryType.Chart;
        }

        /// <summary>
        /// Map Relationship
        /// </summary>
        /// <param name="relationship" />
        /// <returns />
        private Relationship Map(IRepo.RelationShips.IRelationship relationship) =>
            new Relationship
            {
                Dcv = relationship.Dcv,
                IsTypeOfTopic = IsTypeOftopic(relationship.CategoryType),
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

        private static bool IsTypeOftopic(IRepo.enums.CategoryType type) => type switch
        {
            IRepo.enums.CategoryType.With => true,
            IRepo.enums.CategoryType.Goto => true,
            IRepo.enums.CategoryType.From => true,
            IRepo.enums.CategoryType.Who => true,
            IRepo.enums.CategoryType.When => true,
            IRepo.enums.CategoryType.Where => true,
            IRepo.enums.CategoryType.Why => true,
            IRepo.enums.CategoryType.HypTo => true,
            IRepo.enums.CategoryType.HypFrom => true,
            _ => false,
        };

        private static IEnumerable<ISimpleDispatchInstruction> Map(IEnumerable<IRepo.RelationShips.ISimpleDispatchInstruction> dispatchInstructions) =>
            dispatchInstructions.Select(dispatchInstruction => new SimpleDispatchInstruction
            {
                TypeName = dispatchInstruction?.TypeName,
                Dcv = dispatchInstruction?.Dcv,
                Name = dispatchInstruction?.Name,
                Icon = dispatchInstruction?.Icon
            });

        private static IRelationshipElement Map(IRepo.RelationShips.IRelationshipElement topic) =>
            topic == null ? null : new RelationshipElement
            {
                Dcv = topic.Dcv,
                Name = topic.Name,
                Icon = topic.Icon
            };

        private static RelationshipType Map(IRepo.enums.RelationshipType type) =>
            Enum.TryParse<RelationshipType>(type.ToString(), out var result) ? result : RelationshipType.Unknown;

        private static IRepo.enums.RelationshipType Map(RelationshipType type) =>
            Enum.TryParse<IRepo.enums.RelationshipType>(type.ToString(), out var result) ? result : IRepo.enums.RelationshipType.Unknown;

        private static IRepo.enums.CategoryType Map(CategoryType type) =>
            Enum.TryParse<IRepo.enums.CategoryType>(type.ToString(), out var result) ? result : IRepo.enums.CategoryType.Unknown;

        private static CategoryType Map(IRepo.enums.CategoryType type) =>
            Enum.TryParse<CategoryType>(type.ToString(), out var result) ? result : CategoryType.Unknown;
    }
    #endregion
}