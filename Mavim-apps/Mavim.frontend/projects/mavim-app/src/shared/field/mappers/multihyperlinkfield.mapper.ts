import { Field } from '../models/field.model';
import { mapToApiFieldType, mapToFieldType } from './fieldvaluetype';
import { MultiHyperlinkField } from '../models/api/multihyperlinkfield.model';
import { GenericHyperlinkFieldMapper } from './abstract/generic-hyperlink-field.mapper';

export class MultiHyperlinkFieldMapper extends GenericHyperlinkFieldMapper<MultiHyperlinkField> {
	public get getUrlTypeName(): string {
		return 'multihyperlink';
	}

	public getMappedField(field: MultiHyperlinkField): Field {
		return this.mapField(field);
	}

	public getMappedApiField(field: Field): MultiHyperlinkField {
		return this.mapApiField(field);
	}

	protected mapApiField(field: Field): MultiHyperlinkField {
		const data =
			field.data && field.data.length > 0
				? field.data.map((f) => this.mapToUrl(f))
				: [];
		return {
			...field,
			fieldValueType: mapToApiFieldType(field.fieldValueType),
			data
		};
	}

	protected mapField(field: MultiHyperlinkField): Field {
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
