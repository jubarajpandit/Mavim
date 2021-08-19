import { Field } from '../models/field.model';
import { SingleDecimalField } from '../models/api';
import { GenericDecimalFieldMapper } from './abstract';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class DecimalFieldMapper extends GenericDecimalFieldMapper<SingleDecimalField> {
	public get getUrlTypeName(): string {
		return 'decimal';
	}

	public getMappedField(field: SingleDecimalField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): SingleDecimalField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): SingleDecimalField {
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data: field.data ? this.mapToDecimal(field.data[0]) : undefined
		};
	}

	protected mapField(field: SingleDecimalField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data: [this.mapToString(field.data)]
		} as Field;
	}
}
