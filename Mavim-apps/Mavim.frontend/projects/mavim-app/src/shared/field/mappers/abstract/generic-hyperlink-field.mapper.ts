import { GenericFieldMapper } from './generic-field.mapper';

export abstract class GenericHyperlinkFieldMapper<
	T
> extends GenericFieldMapper<T> {
	protected mapToString(url: URL): string {
		return url?.toString() || '';
	}

	protected mapToUrl(url: string): URL {
		if (!url) {
			return undefined;
		}
		return new URL(url);
	}
}
