import { GenericFieldMapper } from './generic-field.mapper';
import { Dictionary } from '@ngrx/entity';

export abstract class GenericlistFieldMapper<T> extends GenericFieldMapper<T> {
	protected mapToOptionsString(list: Dictionary<string>): Dictionary<string> {
		const returnDictionary: Dictionary<string> = {};
		Object.keys(list).forEach((key) => {
			returnDictionary[key] = `${key}:${list[key]}`;
		});
		return returnDictionary;
	}

	protected mapToString(list: Dictionary<string>): string[] {
		if (!list) {
			return undefined;
		}

		return Object.keys(list).map((key) => `${key}:${list[key]}`);
	}

	protected mapToDictionary(list: string): Dictionary<string> {
		const fieldSplit = 2;

		if (!list) {
			return undefined;
		}
		const [key, value] = list.split(':', fieldSplit);
		return { [key]: value ? value : '' } as Dictionary<string>;
	}
}
