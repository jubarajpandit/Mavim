import { NgModule, APP_INITIALIZER, Injector } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { reducer, LayoutEffect } from './+state';
import {
	LayoutListenerService,
	LayoutFacade,
	Favicons,
	BrowserFavicons
} from './service';
import { BROWSER_FAVICONS_CONFIG } from './tokens';
import { EffectsModule } from '@ngrx/effects';
import { FaviconsConfig } from './interfaces';

export function initLayoutListener(
	injector: Injector,
	facade: LayoutFacade
): () => void {
	return (): void => {
		new LayoutListenerService(injector, facade).LoadListener();
	};
}

const iconsConfig: FaviconsConfig = {
	icons: {
		default: {
			type: 'image/png',
			href: '../../assets/images/favicon.png'
		},
		test: {
			type: 'image/x-icon',
			href: '../../assets/icons/test/test-tube.ico'
		},
		word: {
			type: 'image/png',
			href: '../../assets/icons/wopi/word_16x1.png'
		}
	},
	cacheBusting: true
};

@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		StoreModule.forFeature('layout', reducer),
		EffectsModule.forFeature([LayoutEffect])
	],
	exports: [],
	providers: [
		LayoutFacade,
		{
			provide: APP_INITIALIZER,
			useFactory: initLayoutListener,
			deps: [Injector, LayoutFacade],
			multi: true
		},
		// The Favicons is an abstract class that represents the dependency-injection
		// token and the API contract. THe BrowserFavicon is the browser-oriented
		// implementation of the service.
		{
			provide: Favicons,
			useClass: BrowserFavicons
		},
		// The BROWSER_FAVICONS_CONFIG sets up the favicon definitions for the browser-
		// based implementation. This way, the rest of the application only needs to know
		// the identifiers (ie, "happy", "default") - it doesn't need to know the paths
		// or the types. This allows the favicons to be modified independently without
		// coupling too tightly to the rest of the code.
		{
			provide: BROWSER_FAVICONS_CONFIG,
			useValue: iconsConfig
		}
	]
})
export class LayoutModule {}
