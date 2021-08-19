import { environment } from '../../environments/environment';
import { Dictionary } from '@ngrx/entity';

export class UrlUtils {
	public static getUrlByRelativeApiUrl(
		relativeUrl: string,
		replacements: Dictionary<string> = {}
	): string {
		const absoluteUrl: string = environment.baseApiUrl + relativeUrl; // concat-urls
		return UrlUtils.getUrlByAbsoluteApiUrl(absoluteUrl, replacements);
	}

	public static getUrlByAbsoluteApiUrl(
		absoluteUrl: string,
		replacements: Dictionary<string> = {}
	): string {
		return UrlUtils.replaceUrlParts(absoluteUrl, replacements);
	}

	private static replaceUrlParts(
		url: string,
		replacements: Dictionary<string> = {}
	): string {
		let newUrl: string = url;

		if (newUrl) {
			Object.keys(replacements).forEach((key) => {
				const regEx = new RegExp('{' + key + '}', 'ig');
				newUrl = newUrl.replace(regEx, replacements[key]);
			});
		}

		return newUrl;
	}
}
