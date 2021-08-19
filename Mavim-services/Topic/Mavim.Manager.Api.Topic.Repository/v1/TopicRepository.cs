using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Features;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics;
using Mavim.Manager.Api.Topic.Repository.v1.Models;
using Mavim.Manager.Model;
using Mavim.Manager.Server;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRepoEnum = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using System.IO;

namespace Mavim.Manager.Api.Topic.Repository.v1
{
    public class TopicRepository : ITopicRepository
    {
        private readonly IMavimDatabaseModel _model;
        private readonly IFeatureManager _featureManager;

        public TopicRepository(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage, IFeatureManager featureManager)
        {
            _model = dataAccess?.DatabaseModel ?? throw new ArgumentNullException(nameof(dataAccess));
            _model.DataLanguage = new Language(Map(dataLanguage.Type));
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        /// <summary>
        /// Gets the root topic of the Mavim database by establishing the connection with Mavim database using on behalf of access token.
        /// </summary>
        /// <returns></returns>
        public async Task<ITopic> GetRootTopic()
        {
            var codeFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicByCode));
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return Map(_model.RootElement, codeFeatureEnabled, customIconFeatureEnabled);
        }

        public async Task<ITopicPath> GetPathToRoot(string dcvId)
        {
            IDcvId currentDcvId = DcvId.FromDcvKey(dcvId);

            if (string.IsNullOrWhiteSpace(dcvId))
                throw new ArgumentNullException(nameof(dcvId));

            if (currentDcvId == null)
                throw new ArgumentException("DcvId not in right format", dcvId);

            var codeFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicByCode));
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));

            TopicPath topicPath = new TopicPath { Path = new List<IPathItem>(), Data = new List<ITopic>() };
            List<string> path = new List<string>();
            ITopic rootTopic = await GetRootTopic();

            const int maxLoopsForRoot = 40;
            int loop;

            for (loop = 0; loop < maxLoopsForRoot; loop++)
            {
                IElement topic = _model.ElementRepository.GetElement(currentDcvId);
                
                path.Add(topic?.DcvID?.ToString());
                //TODO: Remove Feature Flag => WI: 27606
                topicPath.Data.Add(Map(topic, codeFeatureEnabled, customIconFeatureEnabled));
                //TODO: Remove Feature Flag => WI: 27606
                topicPath.Data.AddRange(topic?.Children?.Select(x => Map(x, codeFeatureEnabled, customIconFeatureEnabled)).ToList());

                if (rootTopic?.Dcv.ToString().ToLower() == currentDcvId?.ToString().ToLower())
                    break;

                currentDcvId = topic?.Parent?.DcvID;
            }

            if (loop >= maxLoopsForRoot)
                throw new TimeoutException("Object exceeded maximum node depth");

            topicPath.Data = topicPath.Data.GroupBy(topic => topic.Dcv).Select(g => g.FirstOrDefault()).ToList();

            path.Reverse();
            topicPath.Path.AddRange(path.Select((dcv, index) => new PathItem { Order = index, DcvId = dcv }).ToList());

            return topicPath;
        }

        /// <summary>
        /// Gets the topic by DCV.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">DcvId not in right format</exception>
        public async Task<ITopic> GetTopicByDcv(string dcvId)
        {
            IDcvId dcv = DcvId.FromDcvKey(dcvId);

            if (string.IsNullOrWhiteSpace(dcvId))
                throw new ArgumentNullException(nameof(dcvId));

            if (dcv == null)
                throw new ArgumentException("DcvId not in right format", dcvId);

            IElement baseTopic = _model.ElementRepository.GetElement(dcv);
            ElementInfo info = _model.Queries.GetElementInfo(dcvId);

            if (baseTopic == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{dcvId}'");

            var codeFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicByCode));
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return Map(baseTopic, info, codeFeatureEnabled, customIconFeatureEnabled);
        }

        /// <summary>
        /// Gets the topic by Code.
        /// </summary>
        /// <param name="topicCode">The Code identifier.</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<ITopic>> GetTopicsByCode(string topicCode)
        {
            if (string.IsNullOrWhiteSpace(topicCode))
                throw new ArgumentNullException(nameof(topicCode));
            
            IElement[] topics = _model.Queries.GetElementsByCode(topicCode);

            var codeFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicByCode));
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return topics.Select(x => Map(x, codeFeatureEnabled, customIconFeatureEnabled)).ToList();
        }        

        /// <summary>
        /// Gets the children by DCV.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">DcvId not in right format</exception>
        public async Task<IEnumerable<ITopic>> GetChildrenByDcv(string dcvId)
        {
            IDcvId dcv = DcvId.FromDcvKey(dcvId);

            if (string.IsNullOrWhiteSpace(dcvId))
                throw new ArgumentNullException(nameof(dcvId));

            if (dcv == null)
                throw new ArgumentException("DcvId not in right format", dcvId);

            IElement baseTopic = _model.ElementRepository.GetElement(dcv);

            if (baseTopic == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{dcvId}'");

            var codeFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicByCode));
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return baseTopic?.Children?.Select(x => Map(x, codeFeatureEnabled, customIconFeatureEnabled)).ToList();
        }

        /// <summary>
        /// Gets the siblings by DCV.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">DcvId not in right format</exception>
        public async Task<IEnumerable<ITopic>> GetSiblingsByDcv(string dcvId)
        {
            IDcvId dcv = DcvId.FromDcvKey(dcvId);

            if (string.IsNullOrWhiteSpace(dcvId))
                throw new ArgumentNullException(nameof(dcvId));

            if (dcv == null)
                throw new ArgumentException("DcvId not in right format", dcvId);

            IElement baseTopic = _model.ElementRepository.GetElement(dcv);

            if (baseTopic == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{dcvId}'");

            IEnumerable<IElement> topics = baseTopic?.Parent?
                                               .Children.Where(element => !element.DcvID.Equals(dcv))
                                           ?? Enumerable.Empty<IElement>();

            var codeFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicByCode));
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));
            //TODO: Remove Feature Flag => WI: 27606
            return topics.Select(x => Map(x, codeFeatureEnabled, customIconFeatureEnabled)).ToList();
        }

        /// <summary>
        /// Gets the relation categories.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">You need to set connection info before getting data</exception>
        public async Task<IEnumerable<ITopic>> GetRelationshipCategories()
        {
            List<ITopic> relationsCategories = new List<ITopic>();
            var codeFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicByCode));
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));            

            foreach (IElement elementFromRoot in _model.RootElement.Children)
            {
                if (elementFromRoot.Type.Type == ELEBuffer.MvmSrv_ELEtpe.MvmSrv_ELEtpe_mid) // is element of type Mavim internal database (yellow cabinet)
                {
                    foreach (IElement elementFromMid in elementFromRoot.Children)
                    {
                        if (elementFromMid.Type.Type == ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_relcat) // is of type relation categories
                        {
                            //TODO: Remove Feature Flag => WI: 27606
                            relationsCategories.Add(Map(elementFromMid, codeFeatureEnabled, customIconFeatureEnabled));
                            //TODO: Remove Feature Flag => WI: 27606
                            relationsCategories.AddRange(elementFromMid.Children.Select(x => Map(x, codeFeatureEnabled, customIconFeatureEnabled)).ToList());
                        }
                    }
                }
            }

            return await Task.FromResult(relationsCategories);
        }

        public async Task<ITopic> UpdateTopicName(string dcvId, string name)
        {
            IDcvId dcv = DcvId.FromDcvKey(dcvId);

            if (string.IsNullOrWhiteSpace(dcvId))
                throw new ArgumentNullException(nameof(dcvId));

            if (dcv == null)
                throw new ArgumentException("DcvId not in right format", dcvId);

            IElement baseTopic = _model.ElementRepository.GetElement(DcvId.FromDcvKey(dcvId));

            if (baseTopic == null)
                throw new RequestNotFoundException($"Could not find topic with DCV '{dcvId}'");

            baseTopic.Name = name;

            var codeFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicByCode));
            //TODO: Remove Feature Flag => WI: 27606
            var customIconFeatureEnabled = await _featureManager.IsEnabledAsync(nameof(TopicFeatureFlags.TopicCustomIcon));

            //TODO: Remove Feature Flag => WI: 27606
            return Map(baseTopic, codeFeatureEnabled, customIconFeatureEnabled);
        }

        /// <summary>
        /// Get Topic Custom Icon By TopicCustomIconId
        /// </summary>
        /// <param name="customIconId"></param>
        /// <returns></returns>

        //TODO: Remove Feature Flag => WI: 27606
        public async Task<byte[]> GetTopicCustomIconByCustomIconId(string customIconId)
        {
            IDcvId dcvId = DcvId.FromDcvKey(customIconId);

            if (string.IsNullOrWhiteSpace(customIconId))
                throw new ArgumentNullException(nameof(customIconId));

            if (dcvId == null)
                throw new ArgumentException("Custom Icon Id not in right format", customIconId);

            ICustomIconElement customIcon = (ICustomIconElement)_model.ElementRepository.GetElement(dcvId);


            if (customIcon == null)
                throw new RequestNotFoundException($"Could not find Custom Icon with DCV '{customIconId}'");

            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                customIcon.GetInternalFile().CopyTo(ms);
                data = ms.ToArray();
            }

            return await Task.Run(() => data);
        }

        #region Private Methods

        //TODO: Remove Feature Flag => WI: 27606
        private ITopic Map(IElement topic, bool codeFeatureEnabled, bool customIconFeatureEnabled)
        {
            var result = Map(topic);
            result.Code = codeFeatureEnabled ? topic.Code : null;
            result.CustomIconId = (customIconFeatureEnabled && Convert.ToBoolean(topic.Visual?.IsCustomIcon)) ? topic.Visual?.CustomIconSource.ToString() : null;
            
            return result;
        }

        /// <summary>
        /// Maps the topic object to the topic data transfer object.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <returns></returns>
        private ITopic Map(IElement topic)
        {
            if (topic == null)
                return null;

            return new Models.Topic
            {
                Dcv = topic.DcvID,
                Parent = topic.Parent?.DcvID.ToString(),
                Name = topic.Name,
                ElementType = Map(topic.Type.Type),
                Icon = topic.Visual?.IconResourceID.ToString("G"),
                IsChart = topic.Type.IsChart,
                Type = Map(topic.Type),
                IsInternal = topic.IsInternal,
                OrderNumber = topic.OrderNumber,
                HasChildren = topic.HasChildren,
                CanDelete = topic.Bizz?.CanDelete ?? false,
                CanCreateChildTopic = topic.Bizz?.CanCreateNewElementLower ?? false,
                CanCreateTopicAfter = topic.Bizz?.CanCreateNewElementUnder ?? false,
                IsInRecycleBin = topic.IsDeleted
            };
        }

        //TODO: Remove Feature Flag => WI: 27606
        private ITopic Map(IElement elementTopic, ElementInfo info, bool codeFeatureEnabled = false, bool customIconFeatureEnabled = false)
        {
            if (elementTopic == null)
                return null;

            //TODO: Remove Feature Flag => WI: 27606
            ITopic topic = Map(elementTopic, codeFeatureEnabled, customIconFeatureEnabled);
            topic.Resource = Map(info);

            return topic;
        }

        private static IType Map(IElementType type) =>
            new Models.Type
            {
                HasSystemName = type.HasSystemName,
                IsImportedVersionsRoot = type.IsImportedVersionsRoot,
                IsImportedVersion = type.IsImportedVersion,
                IsCreatedVersionsRoot = type.IsCreatedVersionsRoot,
                IsCreatedVersion = type.IsCreatedVersion,
                IsRecycleBin = type.IsRecycleBin,
                IsRelationshipsCategoriesRoot = type.IsRelationshipsCategoriesRoot,
                IsExternalReferencesRoot = type.IsExternalReferencesRoot,
                IsObjectsRoot = type.IsObjectsRoot
            };

        private static IRepoEnum.ElementType Map(ELEBuffer.MvmSrv_ELEtpe type)
        {
            return type switch
            {
                ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_vir => IRepoEnum.ElementType.Virtual,
                ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_mav => IRepoEnum.ElementType.MavimElementContainer,
                ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_relcat => IRepoEnum.ElementType.RelationCategories,
                ELEBuffer.MvmSrv_ELEtpe.mvmsrv_eletpe_Objm => IRepoEnum.ElementType.Object,
                ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_Withm => IRepoEnum.ElementType.WithWhat,
                ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_Whom => IRepoEnum.ElementType.Who,
                ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_Wherem => IRepoEnum.ElementType.Where,
                ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_Whenm => IRepoEnum.ElementType.When,
                ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_Whym => IRepoEnum.ElementType.Why,
                ELEBuffer.MvmSrv_ELEtpe.MvmSrv_ELEtpe_Wheretom => IRepoEnum.ElementType.WhereTo,
                ELEBuffer.MvmSrv_ELEtpe.MvmSRV_ELEtpe_rec => IRepoEnum.ElementType.RecycleBin,
                _ => IRepoEnum.ElementType.Unknown
            };
        }

        private static LanguageSupport.MvmSRV_Lang Map(DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                DataLanguageType.Dutch => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_NL,
                DataLanguageType.English => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_EN,
                _ => throw new ArgumentException(string.Format($"unsupported DataLanguage: {dataLanguage}"))
            };

        private static List<TopicResource> Map(ElementInfo info)
        {
            List<TopicResource> resources = new List<TopicResource>();
            if (info.HasChildren) resources.Add(TopicResource.SubTopics);
            if (info.HasDescription) resources.Add(TopicResource.Description);
            if (info.HasFields) resources.Add(TopicResource.Fields);
            if (info.HasRelations) resources.Add(TopicResource.Relations);
            if (info.HasSchemes) resources.Add(TopicResource.Chart);

            return resources;
        }
        #endregion
    }
}
