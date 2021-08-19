import { APIFieldValueType } from '../models/api/';
import { FieldValueType } from '../models/fieldvaluetype.enum';

export function mapToFieldType(fieldType: APIFieldValueType): FieldValueType {
	switch (fieldType) {
		case APIFieldValueType.boolean:
			return FieldValueType.boolean;
		case APIFieldValueType.date:
			return FieldValueType.date;
		case APIFieldValueType.multiDate:
			return FieldValueType.multiDate;
		case APIFieldValueType.decimal:
			return FieldValueType.decimal;
		case APIFieldValueType.multiDecimal:
			return FieldValueType.multiDecimal;
		case APIFieldValueType.list:
			return FieldValueType.list;
		case APIFieldValueType.number:
			return FieldValueType.number;
		case APIFieldValueType.multiNumber:
			return FieldValueType.multiNumber;
		case APIFieldValueType.relationship:
			return FieldValueType.relationship;
		case APIFieldValueType.multiRelationship:
			return FieldValueType.multiRelationship;
		case APIFieldValueType.relationshipList:
			return FieldValueType.relationshipList;
		case APIFieldValueType.text:
			return FieldValueType.text;
		case APIFieldValueType.multiText:
			return FieldValueType.multiText;
		case APIFieldValueType.hyperlink:
			return FieldValueType.hyperlink;
		case APIFieldValueType.multiHyperlink:
			return FieldValueType.multiHyperlink;
		default:
			return FieldValueType.unknown;
	}
}

export function mapToApiFieldType(
	fieldType: FieldValueType
): APIFieldValueType {
	switch (fieldType) {
		case FieldValueType.boolean:
			return APIFieldValueType.boolean;
		case FieldValueType.date:
			return APIFieldValueType.date;
		case FieldValueType.multiDate:
			return APIFieldValueType.multiDate;
		case FieldValueType.decimal:
			return APIFieldValueType.decimal;
		case FieldValueType.multiDecimal:
			return APIFieldValueType.multiDecimal;
		case FieldValueType.list:
			return APIFieldValueType.list;
		case FieldValueType.number:
			return APIFieldValueType.number;
		case FieldValueType.multiNumber:
			return APIFieldValueType.multiNumber;
		case FieldValueType.relationship:
			return APIFieldValueType.relationship;
		case FieldValueType.multiRelationship:
			return APIFieldValueType.multiRelationship;
		case FieldValueType.relationshipList:
			return APIFieldValueType.relationshipList;
		case FieldValueType.text:
			return APIFieldValueType.text;
		case FieldValueType.multiText:
			return APIFieldValueType.multiText;
		case FieldValueType.hyperlink:
			return APIFieldValueType.hyperlink;
		case FieldValueType.multiHyperlink:
			return APIFieldValueType.multiHyperlink;
		default:
			return APIFieldValueType.unknown;
	}
}
