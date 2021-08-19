import { GenericFieldMapper } from './generic-field.mapper';

export abstract class GenericNumberFieldMapper<
	T
> extends GenericFieldMapper<T> {
	protected mapToNumber(value: string): number {
		const baseTen = 10;
		const mappedNumber: number = Number.parseInt(value, baseTen);

		return !Number.isNaN(mappedNumber) ? mappedNumber : undefined;
	}

	protected mapToString(value: number): string {
		return value ? value.toString() : undefined;
	}
}
