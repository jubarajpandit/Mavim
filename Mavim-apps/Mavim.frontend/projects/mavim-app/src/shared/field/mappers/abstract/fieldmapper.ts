import { Field } from '../../models/field.model';
import { ApiField } from '../../models/api/';

export abstract class FieldMapper {
	public abstract get getUrlTypeName(): string;
	public abstract getMappedField(field: ApiField): Field;
	public abstract getMappedApiField(field: Field): ApiField;
}
