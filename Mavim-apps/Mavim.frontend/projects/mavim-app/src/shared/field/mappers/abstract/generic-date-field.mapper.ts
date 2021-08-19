import { GenericFieldMapper } from './generic-field.mapper';
import * as moments from 'moment';

export abstract class GenericDateFieldMapper<T> extends GenericFieldMapper<T> {
	protected mapToString(date: Date): string {
		if (!date) {
			return '';
		}
		return moments(date).isValid() ? moments.utc(date).format() : undefined;
	}

	protected mapToDate(date: string): Date {
		if (!date || !moments(date).isValid()) {
			return undefined;
		}

		return moments(date).toDate();
	}
}
