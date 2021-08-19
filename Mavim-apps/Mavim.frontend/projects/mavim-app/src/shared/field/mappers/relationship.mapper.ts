import { Field } from '../models/field.model';
import { SingleRelationshipField } from '../models/api/';
import { GenericRelationshipFieldMapper } from './abstract';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class RelationshipFieldMapper extends GenericRelationshipFieldMapper<SingleRelationshipField> {
	public get getUrlTypeName(): string {
		return 'relationship';
	}

	public getMappedField(field: SingleRelationshipField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): SingleRelationshipField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): SingleRelationshipField {
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data:
				field.data && field.data.length > 0
					? this.mapToRelationship(field.data[0])
					: undefined
		} as SingleRelationshipField;
	}

	protected mapField(field: SingleRelationshipField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data: field.data ? [this.mapToString(field.data)] : []
		} as Field;
	}
}
