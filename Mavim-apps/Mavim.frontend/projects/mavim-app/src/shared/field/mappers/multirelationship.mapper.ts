import { Field } from '../models/field.model';
import { MultiRelationshipField } from '../models/api';
import { GenericRelationshipFieldMapper } from './abstract';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class MultiRelationshipFieldMapper extends GenericRelationshipFieldMapper<MultiRelationshipField> {
	public get getUrlTypeName(): string {
		return 'multirelationship';
	}

	public getMappedField(field: MultiRelationshipField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): MultiRelationshipField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): MultiRelationshipField {
		const data =
			field.data && field.data.length > 0
				? field.data.map((f) => this.mapToRelationship(f))
				: [];
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data
		} as MultiRelationshipField;
	}

	protected mapField(field: MultiRelationshipField): Field {
		const data =
			field.data && field.data.length > 0
				? field.data.map((f) => this.mapToString(f))
				: [];
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data
		} as Field;
	}
}
