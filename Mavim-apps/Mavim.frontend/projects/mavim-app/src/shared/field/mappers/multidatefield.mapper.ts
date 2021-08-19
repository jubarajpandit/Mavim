import { Field } from '../models/field.model';
import { MultiDateField } from '../models/api';
import { GenericDateFieldMapper } from './abstract';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class MultiDateFieldMapper extends GenericDateFieldMapper<MultiDateField> {
	public get getUrlTypeName(): string {
		return 'multidate';
	}

	public getMappedField(field: MultiDateField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): MultiDateField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): MultiDateField {
		const data =
			field.data && field.data.length > 0
				? field.data.map((f) => this.mapToDate(f))
				: [];
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data
		} as MultiDateField;
	}

	protected mapField(field: MultiDateField): Field {
		const data =
			field.data && field.data.length > 0
				? field.data.map((f) => this.mapToString(f))
				: [];

		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			data
		} as Field;
	}
}
