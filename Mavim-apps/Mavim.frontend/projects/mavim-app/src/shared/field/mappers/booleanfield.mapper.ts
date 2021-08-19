import { Field } from '../models/field.model';
import { SingleBooleanField } from '../models/api/';
import { GenericBoolFieldMapper } from './abstract/generic-bool-field.mapper';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class BooleanFieldMapper extends GenericBoolFieldMapper<SingleBooleanField> {
	public get getUrlTypeName(): string {
		return 'bool';
	}

	public getMappedField(field: SingleBooleanField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): SingleBooleanField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): SingleBooleanField {
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data:
				field.data && field.data.length > 0
					? this.mapToBoolean(field.data[0])
					: undefined
		} as SingleBooleanField;
	}

	protected mapField(field: SingleBooleanField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data: [this.mapToString(field.data)]
		} as Field;
	}
}
