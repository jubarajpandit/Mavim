import { GenericFieldMapper } from './generic-field.mapper';

export abstract class GenericTextFieldMapper<T> extends GenericFieldMapper<T> {
	protected mapToString(text: string): string {
		if (!text) {
			return '';
		}
		return text;
	}

	protected mapToText(text: string): string {
		if (!text) {
			return '';
		}
		return text;
	}
}
