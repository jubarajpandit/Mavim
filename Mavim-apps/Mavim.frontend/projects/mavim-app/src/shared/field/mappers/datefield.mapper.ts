import { Field } from '../models/field.model';
import { SingleDateField } from '../models/api/';
import { GenericDateFieldMapper } from './abstract';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class DateFieldMapper extends GenericDateFieldMapper<SingleDateField> {
	public get getUrlTypeName(): string {
		return 'date';
	}

	public getMappedField(field: SingleDateField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): SingleDateField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): SingleDateField {
		const data =
			field.data && field.data.length > 0
				? this.mapToDate(field.data[0])
				: undefined;
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data
		} as SingleDateField;
	}

	protected mapField(field: SingleDateField): Field {
		const data = [this.mapToString(field.data)];
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data
		} as Field;
	}
}
