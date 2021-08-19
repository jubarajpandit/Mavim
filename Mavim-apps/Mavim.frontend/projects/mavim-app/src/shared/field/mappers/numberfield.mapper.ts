import { Field } from '../models/field.model';
import { SingleNumberField } from '../models/api';
import { GenericNumberFieldMapper } from './abstract/generic-number-field.mapper';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class NumberFieldMapper extends GenericNumberFieldMapper<SingleNumberField> {
	public get getUrlTypeName(): string {
		return 'number';
	}

	public getMappedField(field: SingleNumberField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): SingleNumberField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): SingleNumberField {
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data: field.data ? this.mapToNumber(field.data[0]) : undefined
		} as SingleNumberField;
	}

	protected mapField(field: SingleNumberField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data: [this.mapToString(field.data)]
		} as Field;
	}
}
