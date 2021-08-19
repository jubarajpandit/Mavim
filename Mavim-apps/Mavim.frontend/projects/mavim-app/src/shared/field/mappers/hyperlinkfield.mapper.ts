import { Field } from '../models/field.model';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';
import { GenericHyperlinkFieldMapper } from './abstract/generic-hyperlink-field.mapper';
import { SingleHyperlinkField } from '../models/api/singlehyperlinkfield.model';

export class HyperlinkFieldMapper extends GenericHyperlinkFieldMapper<SingleHyperlinkField> {
	public get getUrlTypeName(): string {
		return 'hyperlink';
	}

	public getMappedField(field: SingleHyperlinkField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): SingleHyperlinkField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): SingleHyperlinkField {
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data:
				field.data && field.data.length > 0
					? this.mapToUrl(field.data[0])
					: undefined
		};
	}

	protected mapField(field: SingleHyperlinkField): Field {
		return {
			...field,
			fieldValueType: mapToFieldType(field.fieldValueType),
			options: undefined,
			data: [this.mapToString(field.data)]
		} as Field;
	}
}
