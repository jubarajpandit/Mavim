import { Field } from '../models/field.model';
import {
	ApiField,
	APIFieldValueType,
	SingleRelationshipListField,
	MultiRelationshipField,
	SingleRelationshipField,
	MultiDateField,
	SingleDateField,
	SingleBooleanField,
	SingleListField,
	SingleTextField,
	MultiTextField,
	SingleNumberField,
	MultiNumberField,
	SingleDecimalField,
	MultiDecimalField,
	SingleHyperlinkField,
	MultiHyperlinkField
} from '../models/api';
import { Fields } from '../models/api/fields';
import { FieldMapperFactory } from './fieldmapperfactory';

export class FieldsMapperFactory {
	public static mapToApiFields(fields: Field[]): Fields {
		const ApiFields = new Fields();
		fields.forEach((field) => {
			const apiField = FieldMapperFactory.mapToApiField(field);
			this.Enrich(apiField, ApiFields);
		});

		return ApiFields;
	}

	private static Enrich(field: ApiField, fields: Fields): void {
		switch (field.fieldValueType) {
			case APIFieldValueType.boolean:
				fields.singleBooleanFields.push(field as SingleBooleanField);
				break;
			case APIFieldValueType.date:
				fields.singleDateFields.push(field as SingleDateField);
				break;
			case APIFieldValueType.multiDate:
				fields.multiDateFields.push(field as MultiDateField);
				break;
			case APIFieldValueType.relationship:
				fields.singleRelationshipFields.push(
					field as SingleRelationshipField
				);
				break;
			case APIFieldValueType.multiRelationship:
				fields.multiRelationshipFields.push(
					field as MultiRelationshipField
				);
				break;
			case APIFieldValueType.relationshipList:
				fields.singleRelationshipListFields.push(
					field as SingleRelationshipListField
				);
				break;
			case APIFieldValueType.list:
				fields.singleListFields.push(field as SingleListField);
				break;
			case APIFieldValueType.text:
				fields.singleTextFields.push(field as SingleTextField);
				break;
			case APIFieldValueType.multiText:
				fields.multiTextFields.push(field as MultiTextField);
				break;
			case APIFieldValueType.number:
				fields.singleNumberFields.push(field as SingleNumberField);
				break;
			case APIFieldValueType.multiNumber:
				fields.multiNumberFields.push(field as MultiNumberField);
				break;
			case APIFieldValueType.decimal:
				fields.singleDecimalFields.push(field as SingleDecimalField);
				break;
			case APIFieldValueType.multiDecimal:
				fields.multiDecimalFields.push(field as MultiDecimalField);
				break;
			case APIFieldValueType.hyperlink:
				fields.singleHyperlinkFields.push(
					field as SingleHyperlinkField
				);
				break;
			case APIFieldValueType.multiHyperlink:
				fields.multiHyperlinkFields.push(field as MultiHyperlinkField);
				break;
			default:
				break;
		}
	}
}
