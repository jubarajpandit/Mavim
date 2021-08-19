﻿using Mavim.Manager.Api.Topic.Services.v1.Mappers.Abstract;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1.Fields;
using IService = Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Services.v1.Mappers
{
    internal class MultiTextFieldMapper : GenericTextFieldMapper<IBusiness.IMultiTextField, IService.IMultiTextField>
    {
        protected override IService.IMultiTextField GetGenericMappedServiceField(IBusiness.IMultiTextField field)
        {
            return new MultiTextField
            {
                FieldSetId = field.FieldSetId,
                FieldId = field.FieldId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                TopicId = field.TopicId,
                SetName = field.SetName,
                FieldName = field.FieldName,
                FieldValueType = Map(field.FieldValueType),
                Required = field.Required,
                Readonly = field.Readonly,
                Usage = field.Usage,
                RelationshipCategory = Map(field.RelationshipCategory),
                Characteristic = Map(field.Characteristic),
                OpenLocation = field.OpenLocation,
                Data = field.Data
            };
        }

        protected override IBusiness.IMultiTextField GetGenericMappedBusinessField(IService.IMultiTextField field)
        {
            return new Business.v1.Models.Fields.MultiTextField
            {
                FieldSetId = field.FieldSetId,
                FieldId = field.FieldId,
                SetOrder = field.SetOrder,
                Order = field.Order,
                TopicId = field.TopicId,
                SetName = field.SetName,
                FieldName = field.FieldName,
                FieldValueType = Map(field.FieldValueType),
                Required = field.Required,
                Readonly = field.Readonly,
                Usage = field.Usage,
                RelationshipCategory = Map(field.RelationshipCategory),
                Characteristic = Map(field.Characteristic),
                OpenLocation = field.OpenLocation,
                Data = field.Data,
            };
        }
    }
}