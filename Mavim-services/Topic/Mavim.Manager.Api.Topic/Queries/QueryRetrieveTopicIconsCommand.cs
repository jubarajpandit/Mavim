using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Commands;
using Mavim.Manager.Api.Topic.Queries.Interfaces;
using Mavim.Manager.Model;
using Mavim.Manager.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Queries
{
    /// <summary>
    /// QueryRetrieveTopicIconsCommand
    /// </summary>
    public class QueryRetrieveTopicIconsCommand : BaseCommand, IQueryRetrieveTopicIconsCommand
    {
        /// <summary>
        /// QueryRetrieveTopicIconsCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public QueryRetrieveTopicIconsCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage) : base(dataAccess, dataLanguage) { }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, string>> Execute(string elementType)
        {
            if (!Enum.TryParse(elementType, true, out ELEBuffer.MvmSrv_ELEtpe modelElementType))
                throw new BadRequestException($"Unknown topic type supplied: {elementType}");

            IElementType type = new ElementType(modelElementType);

            IEnumerable<IIconType> elementTypeIcons = _model.Queries.GetAllowedIconTypes(type);
            if (elementTypeIcons == null)
                return new Dictionary<string, string>();

            return await Task.FromResult(
                elementTypeIcons
                    .Where(x => x != null && x.IsCustomIcon == false)
                    .ToDictionary(x => x.IconResourceID.ToString("G"), x => x.Name)
                );
        }
    }
}
