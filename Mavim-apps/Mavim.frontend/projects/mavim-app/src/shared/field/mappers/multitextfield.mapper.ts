import { Field } from '../models/field.model';
import { MultiTextField } from '../models/api';
import { GenericTextFieldMapper } from './abstract/generic-text-field.mapper';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class MultiTextFieldMapper extends GenericTextFieldMapper<MultiTextField> {
	public get getUrlTypeName(): string {
		return 'multitext';
	}

	public getMappedField(field: MultiTextField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): MultiTextField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): MultiTextField {
		const data = field.data ? [...field.data] : undefined;
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data
		};
	}

	protected mapField(field: MultiTextField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data: field.data ? [...field.data] : []
		} as Field;
	}
}
