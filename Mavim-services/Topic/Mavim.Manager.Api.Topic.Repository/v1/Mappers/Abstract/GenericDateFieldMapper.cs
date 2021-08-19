using System;
using IModel = Mavim.Manager.Model;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract
{
    internal abstract class GenericDateFieldMapper<TModel, TRepo> : GenericFieldMapper<TModel, TRepo> where TModel : IModel.ISimpleField where TRepo : IRepo.IField
    {
        protected DateTime? Map(object fieldValue)
        {
            return DateTime.TryParse(fieldValue?.ToString(), out DateTime result) ? (DateTime?)result : null;
        }
    }
}
