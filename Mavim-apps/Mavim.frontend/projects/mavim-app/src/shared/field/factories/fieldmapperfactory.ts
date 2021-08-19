import {
	BooleanFieldMapper,
	ListFieldMapper,
	NumberFieldMapper,
	MultiNumberFieldMapper,
	DateFieldMapper
} from '../mappers';
import { FieldMapper } from '../mappers/abstract';
import { Field } from '../models/field.model';
import { ApiField, APIFieldValueType } from '../models/api';
import { mapToApiFieldType } from '../mappers/fieldvaluetype';
import {
	TextFieldMapper,
	MultiTextFieldMapper,
	RelationshipFieldMapper,
	MultiDateFieldMapper,
	MultiRelationshipFieldMapper,
	RelationshipListFieldMapper,
	DecimalFieldMapper,
	MultiDecimalFieldMapper,
	HyperlinkFieldMapper,
	MultiHyperlinkFieldMapper
} from '../mappers';
import { FieldValueType } from '../models/fieldvaluetype.enum';

export class FieldMapperFactory {
	public static mapToField(field: ApiField): Field {
		const mapper = this.getFieldMapper(field.fieldValueType);
		if (!mapper) {
			return undefined;
		}

		return mapper.getMappedField(field);
	}

	public static mapToApiField(field: Field): ApiField {
		const fieldType = mapToApiFieldType(field.fieldValueType);
		const mapper = this.getFieldMapper(fieldType);
		if (!mapper) {
			return undefined;
		}

		return mapper.getMappedApiField(field);
	}

	public static mapToPatchUrlType(fieldValueType: FieldValueType): string {
		const fieldType = mapToApiFieldType(fieldValueType);
		const mapper = this.getFieldMapper(fieldType);
		if (!mapper) {
			return undefined;
		}

		return mapper.getUrlTypeName;
	}

	private static getFieldMapper(fieldType: APIFieldValueType): FieldMapper {
		switch (fieldType) {
			case APIFieldValueType.boolean:
				return new BooleanFieldMapper();
			case APIFieldValueType.date:
				return new DateFieldMapper();
			case APIFieldValueType.multiDate:
				return new MultiDateFieldMapper();
			case APIFieldValueType.relationship:
				return new RelationshipFieldMapper();
			case APIFieldValueType.multiRelationship:
				return new MultiRelationshipFieldMapper();
			case APIFieldValueType.relationshipList:
				return new RelationshipListFieldMapper();
			case APIFieldValueType.list:
				return new ListFieldMapper();
			case APIFieldValueType.text:
				return new TextFieldMapper();
			case APIFieldValueType.multiText:
				return new MultiTextFieldMapper();
			case APIFieldValueType.number:
				return new NumberFieldMapper();
			case APIFieldValueType.multiNumber:
				return new MultiNumberFieldMapper();
			case APIFieldValueType.decimal:
				return new DecimalFieldMapper();
			case APIFieldValueType.multiDecimal:
				return new MultiDecimalFieldMapper();
			case APIFieldValueType.hyperlink:
				return new HyperlinkFieldMapper();
			case APIFieldValueType.multiHyperlink:
				return new MultiHyperlinkFieldMapper();
			default:
				return undefined;
		}
	}
}
