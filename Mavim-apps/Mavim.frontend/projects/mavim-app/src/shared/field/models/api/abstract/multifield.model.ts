import { ApiField } from './field.model';
export abstract class MultiField<T> extends ApiField {
	public data: Array<T>;
}
