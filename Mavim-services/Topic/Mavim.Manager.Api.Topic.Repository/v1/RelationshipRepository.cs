using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using Mavim.Manager.Api.Topic.Repository.v1.Models;
using Mavim.Manager.Model;
using Mavim.Manager.Server;
using Mavim.Manager.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Repository.v1
{
    public class RelationshipRepository : IRelationshipsRepository
    {
        private static ILogger<RelationshipRepository> _logger;

        private readonly IMavimDatabaseModel _model;

        public RelationshipRepository(IMavimDbDataAccess dataAccess, ILogger<RelationshipRepository> logger, IDataLanguage dataLanguage)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _model = dataAccess?.DatabaseModel ?? throw new ArgumentNullException(nameof(dataAccess));
            _model.DataLanguage = new Language(Map(dataLanguage.Type));
        }

        /// <summary>
        /// Gets the relations by DCV from Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="dcvId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IRelationship>> GetRelationships(string dcvId)
        {
            IDcvId dcv = DcvId.FromDcvKey(dcvId);

            if (string.IsNullOrWhiteSpace(dcvId))
                throw new ArgumentNullException(nameof(dcvId));

            if (dcv == null)
                throw new ArgumentException($"DcvId not in right format", dcvId);

            IElement baseTopic = _model.ElementRepository.GetElement(dcv);

            if (baseTopic == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{dcvId}'");

            IEnumerable<IRelation> relationships = baseTopic.Relations.ToList();

            return await Task.FromResult(relationships.Select(r => Map(r, dcvId)).ToList());
        }

        /// <summary>
        /// Create a relations by DCV from Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <param name="fromTopicDcv"></param>
        /// <param name="toTopicDcv"></param>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public async Task<IRelationship> CreateRelationship(string fromTopicDcv, string toTopicDcv, RelationshipType relationshipType)
        {
            IDcvId fromDcvId = DcvId.FromDcvKey(fromTopicDcv);
            IDcvId toDcvId = DcvId.FromDcvKey(toTopicDcv);

            if (string.IsNullOrWhiteSpace(fromTopicDcv))
                throw new ArgumentNullException(nameof(fromTopicDcv));

            if (fromDcvId == null)
                throw new ArgumentException("DcvId not in right format", fromTopicDcv);

            if (string.IsNullOrWhiteSpace(toTopicDcv))
                throw new ArgumentNullException(nameof(toTopicDcv));

            if (toDcvId == null)
                throw new ArgumentException("DcvId not in right format", toTopicDcv);

            IElement fromElement = _model.ElementRepository.GetElement(fromDcvId);
            IElement toElement = _model.ElementRepository.GetElement(toDcvId);

            if (fromElement == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{fromTopicDcv}'");

            if (toElement == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{toElement}'");

            IMavimDatabaseModelResultCommand relationCreateCommand = _model.Factories.CommandFactory.CreateCreateRelationCommand(fromElement, toElement, Map(relationshipType));

            if (!relationCreateCommand.CanExecute())
                throw new Exception("Cannot create relation");

            IRelation createdRelation = (IRelation)relationCreateCommand.Execute(new ProgressDummy());

            return await Task.FromResult(Map(createdRelation, fromTopicDcv));
        }

        /// <summary>
        /// Deletes the relation.
        /// </summary>
        /// <param name="dcvId"></param>
        /// <param name="relationshipId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRelationship(string dcvId, string relationshipId)
        {
            IDcvId dcv = DcvId.FromDcvKey(dcvId);

            if (string.IsNullOrWhiteSpace(dcvId))
                throw new ArgumentNullException(nameof(dcvId));

            if (dcv == null)
                throw new ArgumentException("DcvId is of invalid format", dcvId);

            if (String.IsNullOrWhiteSpace(relationshipId))
                throw new ArgumentException("relationshipId is of invalid format", relationshipId);

            IElement baseTopic = _model.ElementRepository.GetElement(dcv);

            if (baseTopic == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{dcvId}'");

            IEnumerable<IRelation> relations = baseTopic?.Relations;
            IRelation relationToDelete = relations?.FirstOrDefault(r => r.DcvId.ToString().Equals(relationshipId));

            if (relationToDelete == null)
                throw new RequestNotFoundException($"Could not find relation with ID '{relationshipId}'");

            IMavimDatabaseModelResultCommand deleteCommand = _model.Factories.CommandFactory.CreateDeleteRelationCommand(relationToDelete);
            bool result = (bool)deleteCommand.Execute(new ProgressDummy());

            return await Task.FromResult(result);
        }

        #region Private Methods
        /// <summary>
        /// MapRelation
        /// </summary>
        /// <param name="relation"></param>
        /// <param name="dcvId"></param>
        /// <returns></returns>
        private static IRelationship Map(IRelation relation, string dcvId)
        {
            IElement withElement = GetLinkedElementFromRelationByDcvId(relation, dcvId);
            IElement fromElement = GetLinkedElementFromRelationByDcvId(relation, withElement.DcvID.ToString());

            return new Relationship
            {
                Dcv = relation.DcvId.ToString(),
                Category = relation.GetCategory(fromElement).Name,
                RelationshipType = Map(relation.Type.Type),
                CategoryType = Map(relation.GetCategory(fromElement).Type),
                Icon = relation.GetCategory(fromElement)?.GetRelationCategoryImageResourceID().ToString("G"),
                UserInstruction = Map(relation?.UserInstruction),
                DispatchInstructions = Map(relation),
                Characteristic = Map(relation.Characteristic),
                WithElement = Map(withElement),
                WithElementParent = Map(withElement.Parent),
                IsAttributeRelation = relation.IsAttributeRelation,
                IsDestroyed = relation.IsDestroyed
            };
        }

        private static CategoryType Map(RELBuffer.MvmSrv_CATtpe type)
        {
            return type switch
            {
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_With => CategoryType.With,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Goto => CategoryType.Goto,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_From => CategoryType.From,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Who => CategoryType.Who,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_When => CategoryType.When,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Where => CategoryType.Where,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Why => CategoryType.Why,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_HypTo => CategoryType.HypTo,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_HypFrom => CategoryType.HypFrom,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Chart => CategoryType.Chart,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Matrix => CategoryType.Matrix,
                RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Report => CategoryType.Report,
                _ => CategoryType.Unknown
            };
        }

        private static RelationshipType Map(RELBuffer.MvmSrv_RELtpe type)
        {
            return type switch
            {
                RELBuffer.MvmSrv_RELtpe.MvmSrv_RELtpe_With => RelationshipType.WithWhat,
                RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_who => RelationshipType.Who,
                RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_when => RelationshipType.When,
                RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_Where => RelationshipType.Where,
                RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_why => RelationshipType.Why,
                RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_WhereTo => RelationshipType.WhereTo,
                _ => RelationshipType.Unknown,
            };
        }

        private static RELBuffer.MvmSrv_RELtpe Map(RelationshipType type)
        {
            return type switch
            {
                RelationshipType.WithWhat => RELBuffer.MvmSrv_RELtpe.MvmSrv_RELtpe_With,
                RelationshipType.Who => RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_who,
                RelationshipType.When => RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_when,
                RelationshipType.Where => RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_Where,
                RelationshipType.Why => RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_why,
                RelationshipType.WhereTo => RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_WhereTo,
                _ => RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_non,
            };
        }

        /// <summary>
        /// GetLinkedElementFromRelationByDcvId
        /// </summary>
        /// <param name="relation"></param>
        /// <param name="dcvId"></param>
        /// <returns></returns>
        private static IElement GetLinkedElementFromRelationByDcvId(IRelation relation, string dcvId)
        {
            return relation.ToElement.DcvID.ToString().Equals(dcvId) ? relation.FromElement : relation.ToElement;
        }

        /// <summary>
        /// Maps to dispatch instruction.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <returns></returns>
        private static IEnumerable<ISimpleDispatchInstruction> Map(IRelation relation)
        {
            return relation.DispatchInstructions.Select(Map)
                                                .Where(dispatchInstruction => dispatchInstruction != null)
                                                .ToList();
        }

        /// <summary>
        /// MapSimpleDispatchInstruction
        /// </summary>
        /// <param name="dispatchInstruction"></param>
        /// <returns></returns>
        private static ISimpleDispatchInstruction Map(IDispatchInstruction dispatchInstruction)
        {
            if (dispatchInstruction == null || dispatchInstruction.IsDestroyed)
                return null;

            IElement element = dispatchInstruction.SaveSendElement ?? dispatchInstruction.DispatchTypeElement;
            if (element == null)
            {
                _logger.LogInformation("Dispatch instruction element is null.");
                return null;
            }

            return new SimpleDispatchInstruction()
            {
                Dcv = element.DcvID.ToString(),
                TypeName = dispatchInstruction.DispatchTypeElement?.Name,
                Name = element.Name,
                Icon = element.Visual?.IconResourceID.ToString("G"),
                IsPublic = element.Type?.IsPublic ?? false
            };
        }

        /// <summary>
        /// Maps to relation topic.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private static IRelationshipElement Map(IElement element)
        {
            if (element == null)
                return null;

            return new RelationshipElement
            {
                Dcv = element.DcvID.ToString(),
                Name = element.Name,
                Icon = element.Visual?.IconResourceID.ToString("G"),
                IsPublic = element.Type?.IsPublic ?? false
            };
        }
        private static LanguageSupport.MvmSRV_Lang Map(DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                DataLanguageType.Dutch => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_NL,
                DataLanguageType.English => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_EN,
                _ => throw new ArgumentException(String.Format("unsupported DataLanguage: {0}", dataLanguage.ToString()))
            };
        #endregion
    }
}
