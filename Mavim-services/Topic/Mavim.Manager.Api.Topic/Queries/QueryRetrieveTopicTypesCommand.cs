using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Commands;
using Mavim.Manager.Api.Topic.Queries.Interfaces;
using Mavim.Manager.Api.Topic.v1.Models;
using Mavim.Manager.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Queries
{
    /// <summary>
    /// QueryRetrieveTopicTypesCommand
    /// </summary>
    public class QueryRetrieveTopicTypesCommand : BaseCommand, IQueryRetrieveTopicTypesCommand
    {
        private readonly string _defaultIcon = "TreeIconID_None";

        /// <summary>
        /// QueryRetrieveTopicTypesCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public QueryRetrieveTopicTypesCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, ElementTypeInfo>> Execute(string topicId)
        {
            IDcvId topicDcvId = DcvId.FromDcvKey(topicId);
            if (topicDcvId == null)
                throw new BadRequestException($"Supplied topicId format is invalid: {topicId}");

            IElement parent = _model.ElementRepository.GetElement(topicDcvId);
            if (parent == null)
                throw new BadRequestException($"Supplied parent topic to get topic types not found: {topicDcvId}");

            IEnumerable<IElementType> types = _model.Queries.GetAllowedChildElementTypes(parent);
            if (types == null)
                return new Dictionary<string, ElementTypeInfo>();

            IDictionary<string, ElementTypeInfo> typeDictionary = types
                    .Where(type => type != null)
                    .ToDictionary(x => x.Type.ToString(), x => new ElementTypeInfo { Name = x.GetName(_model), IsSystemName = x.HasSystemName, ResourceId = !x.IsCustomIcon ? x.GetIcon(_model)?.IconResourceID.ToString("G") : _defaultIcon });

            return await Task.FromResult(typeDictionary);
        }
    }
}
