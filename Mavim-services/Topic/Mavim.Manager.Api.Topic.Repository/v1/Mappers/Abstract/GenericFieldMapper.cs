using System;
using IModel = Mavim.Manager.Model;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract
{
    internal abstract class GenericFieldMapper<TModel, TRepo> : FieldMapperBase where TModel : IModel.ISimpleField where TRepo : IRepo.IField
    {
        internal override IRepo.IField GetMappedRepoField(IModel.ISimpleField field)
        {
            if (!(field is TModel fieldModel)) throw new Exception(nameof(field));
            return GetGenericMappedServiceField(fieldModel);
        }

        internal override object[] ConvertToArrayObject(IRepo.IField field, IModel.ISimpleField simpleField = null)
        {
            if (!(field is TRepo fieldRepo)) throw new Exception(nameof(field));

            return GetGenericMappedRepoField(fieldRepo, simpleField);
        }

        protected abstract TRepo GetGenericMappedServiceField(TModel field);

        protected abstract object[] GetGenericMappedRepoField(TRepo field, IModel.ISimpleField simpleField = null);


    }
}
