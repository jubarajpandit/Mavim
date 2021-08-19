using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract;
using Mavim.Manager.Model;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers.Factory
{
    public class FieldMapperFactory
    {
        private readonly IMavimDbDataAccess _dataAccess;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IFeatureManager _featureManager;
        private readonly string hyperlinkFeature = "Hyperlink";
        private readonly string fieldOpenLocationFeature = "FieldOpenLocation";

        public FieldMapperFactory(IFeatureManager featureManager, ILoggerFactory loggerFactory, IMavimDbDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        internal async Task<FieldMapperBase> GetFieldMapper(FieldType fieldType)
        {
            if (!Enum.IsDefined(typeof(FieldType), fieldType))
                throw new InvalidEnumArgumentException(nameof(fieldType), (int)fieldType, typeof(FieldType));

            return fieldType switch
            {
                FieldType.Unknown => throw new NotImplementedException(),
                FieldType.Text => (FieldMapperBase)new TextFieldMapper(),
                FieldType.MultiText => new MultiTextFieldMapper(),
                FieldType.Number => new NumberFieldMapper(),
                FieldType.MultiNumber => new MultiNumberFieldMapper(),
                FieldType.Decimal => new DecimalFieldMapper(),
                FieldType.MultiDecimal => new MultiDecimalFieldMapper(),
                FieldType.Boolean => new BooleanFieldMapper(),
                FieldType.Date => new DateFieldMapper(),
                FieldType.MultiDate => new MultiDateFieldMapper(),
                FieldType.List => new ListFieldMapper(),
                FieldType.Relationship => await _featureManager.IsEnabledAsync(fieldOpenLocationFeature) ? new RelationshipFieldMapper(_dataAccess) : new RelationshipFieldMapperOld(_dataAccess),
                FieldType.MultiRelationship => await _featureManager.IsEnabledAsync(fieldOpenLocationFeature) ? new MultiRelationshipFieldMapper(_dataAccess) : new MultiRelationshipFieldMapperOld(_dataAccess),
                FieldType.RelationshipList => new RelationshipListFieldMapper(),
                FieldType.MultiHyperlink => new MultiHyperlinkFieldMapper(_loggerFactory),
                FieldType.Hyperlink => new HyperlinkFieldMapper(_loggerFactory),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        internal async Task<FieldMapperBase> GetFieldMapper(ISimpleField simpleField)
        {
            if (simpleField == null) throw new ArgumentNullException(nameof(simpleField));

            bool? isMulti = null;
            if (simpleField is IMultiValueSimpleField)
                isMulti = true;
            if (simpleField is ISingleValueSimpleField)
                isMulti = false;
            if (isMulti == null)
                throw new Exception("Expected ISingleValueSimpleField or IMultiValueSimpleField");

            switch (simpleField.FieldDefinition.VarType)
            {
                case FieldVarType.Text:
                    if (await _featureManager.IsEnabledAsync(hyperlinkFeature))
                    {
                        var textFieldDefinition = simpleField.FieldDefinition is IFieldTextDefinition ? simpleField.FieldDefinition as IFieldTextDefinition : null;
                        if (textFieldDefinition != null && textFieldDefinition.UriFormat)
                            return ((bool)isMulti ? (FieldMapperBase)new MultiHyperlinkFieldMapper(_loggerFactory) : new HyperlinkFieldMapper(_loggerFactory));
                    }

                    return ((bool)isMulti ? (FieldMapperBase)new MultiTextFieldMapper() : new TextFieldMapper());
                case FieldVarType.Number:
                    return ((bool)isMulti ? (FieldMapperBase)new MultiNumberFieldMapper() : new NumberFieldMapper());
                case FieldVarType.Decimal:
                    return ((bool)isMulti
                        ? (FieldMapperBase)new MultiDecimalFieldMapper()
                        : new DecimalFieldMapper());
                case FieldVarType.Boolean:
                    return new BooleanFieldMapper();
                case FieldVarType.Date:
                    return ((bool)isMulti ? (FieldMapperBase)new MultiDateFieldMapper() : new DateFieldMapper());
                case FieldVarType.List:
                    return new ListFieldMapper();
                case FieldVarType.Relation:
                    return ((bool)isMulti
                        ? (FieldMapperBase)(await _featureManager.IsEnabledAsync(fieldOpenLocationFeature) ? new MultiRelationshipFieldMapper(_dataAccess) : new MultiRelationshipFieldMapperOld(_dataAccess))
                        : (await _featureManager.IsEnabledAsync(fieldOpenLocationFeature) ? new RelationshipFieldMapper(_dataAccess) : new RelationshipFieldMapperOld(_dataAccess)));
                case FieldVarType.RelationList:
                    return new RelationshipListFieldMapper();
                case FieldVarType.Unknown:
                    //case FieldVarType.Integer:
                    //case FieldVarType.Time:
                    //case FieldVarType.InternalFile:
                    // TODO: add logging: WI:19087
                    throw new NotImplementedException();
                default:
                    // returning null now because we don't want to throw an exception when a fieldtype does not have a mapper
                    return null;
            }
        }
    }
}
