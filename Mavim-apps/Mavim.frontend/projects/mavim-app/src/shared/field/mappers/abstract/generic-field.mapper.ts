import { FieldMapper } from './fieldmapper';
import { Field } from '../../models/field.model';

export abstract class GenericFieldMapper<T> extends FieldMapper {
	protected abstract mapField(field: T): Field;
	protected abstract mapApiField(field: Field): T;
}
