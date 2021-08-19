import { Field } from '../models/field.model';
import { MultiNumberField } from '../models/api';
import { GenericNumberFieldMapper } from './abstract/generic-number-field.mapper';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class MultiNumberFieldMapper extends GenericNumberFieldMapper<MultiNumberField> {
	public get getUrlTypeName(): string {
		return 'multinumber';
	}

	public getMappedField(field: MultiNumberField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): MultiNumberField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): MultiNumberField {
		const data = field.data
			? [...field.data].map((d) => this.mapToNumber(d))
			: undefined;
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data
		} as MultiNumberField;
	}

	protected mapField(field: MultiNumberField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data: field.data
				? [...field.data].map((d) => this.mapToString(d))
				: []
		} as Field;
	}
}
