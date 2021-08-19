import { GenericFieldMapper } from './generic-field.mapper';

export abstract class GenericBoolFieldMapper<T> extends GenericFieldMapper<T> {
	private readonly trueValue = 'true';

	protected mapToString(bool: boolean): string {
		if (bool === null || bool === undefined) {
			return undefined;
		}

		return bool.toString();
	}

	protected mapToBoolean(bool: string): boolean {
		if (!bool) {
			return undefined;
		}

		return bool === this.trueValue;
	}
}
