using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using Mavim.Manager.Api.Topic.Repository.v1.Models;
using Mavim.Manager.Model;
using System.Collections.Generic;
using System.Linq;
using IModel = Mavim.Manager.Model;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract
{
    internal abstract class GenericRelationshipListFieldMapper<TModel, TRepo> : GenericFieldMapper<TModel, TRepo> where TModel : IModel.ISimpleField where TRepo : IRepo.IField
    {
        protected Dictionary<string, IRelationshipElement> Map(IElement element)
        {
            if (element == null)
                return new Dictionary<string, IRelationshipElement>();

            return new Dictionary<string, IRelationshipElement>
            {
                {element.DcvID.ToString(), GetRelationFieldValue(element)}
            };
        }

        protected Dictionary<string, IRelationshipElement> Map(IFieldDefinition fieldDefinition)
        {
            if (fieldDefinition == null) return new Dictionary<string, IRelationshipElement>();

            Model.IFieldRelationListDefinition fieldRelationListDefinition = (IFieldRelationListDefinition)fieldDefinition;

            if (fieldRelationListDefinition?.ListParent == null)
                return new Dictionary<string, IRelationshipElement>();

            Dictionary<string, IRelationshipElement> options = fieldRelationListDefinition?.ListParent?.Children?.ToDictionary(
                 element => element?.DcvID?.ToString(), element =>
                 {
                     IRelationshipElement retVal = new RelationshipElement
                     {
                         Dcv = element?.DcvID?.ToString(),
                         Icon = element?.Visual.IconResourceID.ToString("G"),
                         Name = element?.Name
                     };
                     return retVal;
                 }
             );

            return options;
        }

        protected IRelationshipElement GetRelationFieldValue(IElement topic)
        {
            if (topic == null)
                return null;

            return new RelationshipElement
            {
                Dcv = topic.DcvID?.ToString(),
                Icon = topic.Visual?.IconResourceID.ToString("G"),
                Name = topic.Name
            };

        }
    }
}
