import { Field } from '../models/field.model';
import { SingleListField } from '../models/api/';
import { GenericlistFieldMapper } from './abstract';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class ListFieldMapper extends GenericlistFieldMapper<SingleListField> {
	public get getUrlTypeName(): string {
		return 'list';
	}

	public getMappedField(field: SingleListField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): SingleListField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): SingleListField {
		return {
			...field,
			options: undefined,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data: field.data ? this.mapToDictionary(field.data[0]) : undefined
		} as SingleListField;
	}

	protected mapField(field: SingleListField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: this.mapToOptionsString(field.options),
			data: this.mapToString(field.data)
		} as Field;
	}
}
