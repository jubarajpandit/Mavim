using System.Collections.Generic;
using System.Linq;
using IModel = Mavim.Manager.Model;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract
{
    internal abstract class GenericListFieldMapper<TModel, TRepo> : GenericFieldMapper<TModel, TRepo> where TModel : IModel.ISimpleField where TRepo : IRepo.IField
    {
        protected Dictionary<string, string> Map(IModel.IFieldDefinition fieldDefinition)
        {
            IModel.IFieldListDefinition fieldListDefinition = (IModel.IFieldListDefinition)fieldDefinition;
            if (fieldListDefinition == null)
            {
                // TODO: add logging: WI:19087
                return new Dictionary<string, string>();
            }
            if (fieldListDefinition.ListItems == null)
                return new Dictionary<string, string>();

            Dictionary<string, string> options = fieldListDefinition.ListItems.
                Select((item) => new { key = item.ID?.ToString(), value = item.GetDisplayText() })
                .ToDictionary(x => x.key, x => x.value);
            return options;
        }

        protected Dictionary<string, string> Map(object fieldValue)
        {
            IModel.IFieldDefinitionListItem fieldDefinitionListItem = (IModel.IFieldDefinitionListItem)fieldValue;
            if (fieldDefinitionListItem == null)
            {
                // TODO: add logging: WI:19087
                return null;
            }
            return new Dictionary<string, string>
            {
                {
                    fieldDefinitionListItem.ID, fieldDefinitionListItem.GetDisplayText()
                }
            };
        }
    }
}
