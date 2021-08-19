import { Field } from '../models/field.model';
import { SingleTextField } from '../models/api';
import { GenericTextFieldMapper } from './abstract/generic-text-field.mapper';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class TextFieldMapper extends GenericTextFieldMapper<SingleTextField> {
	public get getUrlTypeName(): string {
		return 'text';
	}

	public getMappedField(field: SingleTextField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): SingleTextField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): SingleTextField {
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data: field.data ? this.mapToText(field.data[0]) : undefined
		};
	}

	protected mapField(field: SingleTextField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data: [this.mapToString(field.data)]
		} as Field;
	}
}
