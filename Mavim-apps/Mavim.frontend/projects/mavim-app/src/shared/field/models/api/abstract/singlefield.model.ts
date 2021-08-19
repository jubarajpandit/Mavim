import { ApiField } from './field.model';
export abstract class SingleField<T> extends ApiField {
	public data?: T;
}
