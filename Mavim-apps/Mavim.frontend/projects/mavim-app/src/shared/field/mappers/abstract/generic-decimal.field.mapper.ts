import { GenericFieldMapper } from './generic-field.mapper';
import { RegexUtils } from '../../../utils/regex.utils';

export abstract class GenericDecimalFieldMapper<
	T
> extends GenericFieldMapper<T> {
	protected mapToString(decimal: number): string {
		if (decimal === undefined || decimal === null) {
			return undefined;
		}
		return decimal.toLocaleString();
	}

	protected mapToDecimal(value: string): number {
		return GenericDecimalFieldMapper.parseDecimal(value);
	}

	public static parseDecimal(value: string): number {
		if (value === null || value === undefined) {
			return undefined;
		}
		const decimalString: string = value
			.replace(RegexUtils.getDottedThousandsSeperators(), '')
			.replace(',', '.');
		const decimal = Number(decimalString);

		if (
			RegexUtils.isNumeric().test(decimalString) &&
			!Number.isNaN(decimal) &&
			Number.isFinite(decimal)
		) {
			return decimal;
		}
		return undefined;
	}
}
