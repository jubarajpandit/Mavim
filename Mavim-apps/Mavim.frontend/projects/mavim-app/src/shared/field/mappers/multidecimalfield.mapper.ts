import { Field } from '../models/field.model';
import { MultiDecimalField } from '../models/api';
import { GenericDecimalFieldMapper } from './abstract';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class MultiDecimalFieldMapper extends GenericDecimalFieldMapper<MultiDecimalField> {
	public get getUrlTypeName(): string {
		return 'multidecimal';
	}

	public getMappedField(field: MultiDecimalField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): MultiDecimalField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): MultiDecimalField {
		const data =
			field.data && field.data.length > 0
				? field.data.map((d) => this.mapToDecimal(d))
				: [];
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data
		} as MultiDecimalField;
	}

	protected mapField(field: MultiDecimalField): Field {
		const data =
			field.data && field.data.length > 0
				? field.data.map((d) => this.mapToString(d))
				: [];

		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			data
		} as Field;
	}
}
