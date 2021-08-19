import { Field } from '../models/field.model';
import { SingleRelationshipListField } from '../models/api/';
import { GenericRelationshiplistFieldMapper } from './abstract';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';

export class RelationshipListFieldMapper extends GenericRelationshiplistFieldMapper<SingleRelationshipListField> {
	public get getUrlTypeName(): string {
		return 'relationshiplist';
	}

	public getMappedField(field: SingleRelationshipListField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): SingleRelationshipListField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): SingleRelationshipListField {
		return {
			...field,
			options: undefined,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data:
				field.data && field.data.length > 0
					? this.mapToApiModel(field.data)
					: undefined
		} as SingleRelationshipListField;
	}

	protected mapField(field: SingleRelationshipListField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: this.mapToOptionsString(field.options),
			data: this.mapToString(field.data)
		} as Field;
	}
}
