import { Inject, Injectable } from '@angular/core';
import { BROWSER_FAVICONS_CONFIG } from '../tokens/browser-favicons-config.token';
import { IconsConfig } from '../interfaces/IconsConfig';
import { FaviconsConfig } from '../interfaces/FaviconsConfig';

// This abstract class acts as both the interface for implementation (for any developer
// that wants to create an alternate implementation) and as the dependency-injection
// token that the rest of the application can use.
export abstract class Favicons {
	public abstract activate(name: string): void;
	public abstract reset(): void;
}

// I provide the browser-oriented implementation of the Favicons class.
@Injectable()
export class BrowserFavicons implements Favicons {
	// I initialize the Favicons service.
	public constructor(
		@Inject(BROWSER_FAVICONS_CONFIG) config: FaviconsConfig
	) {
		this.elementId = 'favicons-service-injected-node';
		// eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
		this.icons = Object.assign(Object.create(null), config.icons);
		this.useCacheBusting = config.cacheBusting || false;

		// Since the document may have a static favicon definition, we want to strip out
		// any exisitng elements that are attempting to define a favicon. This way, there
		// is only one favicon element on the page at a time.
		this.removeExternalLinkElements();
	}

	private readonly elementId: string;
	private readonly icons: IconsConfig;
	private readonly useCacheBusting: boolean;

	// I activate the favicon with the given name / identifier.
	public activate(name: string): void {
		if (!this.icons[name]) {
			throw new Error(`Favicon for [ ${name} ] not found.`);
		}

		this.setNode(this.icons[name].type, this.icons[name].href);
	}

	// I activate the default favicon (with isDefault set to True).
	public reset(): void {
		Object.keys(this.icons).forEach((name) => {
			const icon = this.icons[name];

			if (icon.isDefault) {
				this.setNode(icon.type, icon.href);
				return;
			}
		});

		// If we made it this far, none of the favicons were flagged as default. In that
		// case, let's just remove the favicon node altogether.
		this.removeNode();
	}

	// I inject the favicon element into the document header.
	private addNode(type: string, href: string): void {
		const linkElement = document.createElement('link');
		linkElement.setAttribute('id', this.elementId);
		linkElement.setAttribute('rel', 'icon');
		linkElement.setAttribute('type', type);
		linkElement.setAttribute('href', href);
		document.head.appendChild(linkElement);
	}

	// I return an augmented HREF value with a cache-busting query-string parameter.
	private cacheBustHref(href: string): string {
		const augmentedHref =
			href.indexOf('?') === -1
				? `${href}?faviconCacheBust=${Date.now()}`
				: `${href}&faviconCacheBust=${Date.now()}`;

		return augmentedHref;
	}

	// I remove any favicon nodes that are not controlled by this service.
	private removeExternalLinkElements(): void {
		// eslint-disable-next-line quotes, @typescript-eslint/quotes
		const linkElements = document.querySelectorAll(
			"link[ rel ~= 'icon' i]"
		);

		Array.from(linkElements).forEach((linkElement) => {
			linkElement.parentNode.removeChild(linkElement);
		});
	}

	// I remove the favicon node from the document header.
	private removeNode(): void {
		const linkElement = document.head.querySelector('#' + this.elementId);

		if (linkElement) {
			document.head.removeChild(linkElement);
		}
	}

	// I remove the existing favicon node and inject a new favicon node with the given
	// element settings.
	private setNode(type: string, href: string): void {
		const augmentedHref = this.useCacheBusting
			? this.cacheBustHref(href)
			: href;

		this.removeNode();
		this.addNode(type, augmentedHref);
	}
}
