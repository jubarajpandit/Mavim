import { Injectable } from '@angular/core';
import { RoutingSessionStoreService } from './routing-session-store.service';
import { NavigationEnd, Router } from '@angular/router';
import { filter, take } from 'rxjs/operators';
import { RoutingService } from './routing.service';
import { RoutingFacade } from './routing.facade';
import { LanguageFacade } from '../../language/service/language.facade';
import { LanguageService } from '../../language/service/language.service';
import { Language } from '../../language/enums/language.enum';

@Injectable({
	providedIn: 'root'
})
export class RoutingBrowserService {
	public constructor(
		private readonly router: Router,
		private readonly routeSessionStore: RoutingSessionStoreService,
		private readonly routingService: RoutingService,
		private readonly routingFacade: RoutingFacade,
		private readonly languageFacade: LanguageFacade,
		private readonly languageService: LanguageService
	) {
		this.initRouterStore();
		this.lisenToBrowserNavigation();
	}

	private initRouterStore(): void {
		const {
			getQueue: sessionDcvIds,
			getLastDcv: lastSessionStoreItem,
			getSecondLastDcv: secondLastSessionStoreItem
		} = this.routeSessionStore;
		const { currentDcvIds } = this.routingService;
		const firstCurrentDcv = this.getIndexDcvIdfromQueue(currentDcvIds, 0);
		const secondCurrentDcv = this.getIndexDcvIdfromQueue(currentDcvIds, 1);

		if (
			((!currentDcvIds && currentDcvIds.length === 0) || // No outlets avalible
				(firstCurrentDcv === secondLastSessionStoreItem &&
					lastSessionStoreItem === secondCurrentDcv)) && // sessionstore equal outlets
			sessionDcvIds?.length > 0 // sessionDcvIds are initialized
		) {
			this.routingFacade.updateQueue([...sessionDcvIds]); // restore session to store
		} else {
			this.routingFacade.init(currentDcvIds);

			if (firstCurrentDcv === secondCurrentDcv) {
				this.routingFacade.home(firstCurrentDcv);
			}
		}
	}

	private lisenToBrowserNavigation(): void {
		this.router.events
			.pipe(filter((event) => event instanceof NavigationEnd))
			.subscribe(() => {
				this.checkIfRouteIsChanged();
				this.setLanguageIfLanguageIsChanged();
			});
	}

	private setLanguageIfLanguageIsChanged(): void {
		this.languageFacade.language.pipe(take(1)).subscribe((language) => {
			const languageFromUrl: Language =
				this.languageService.getLanguageFromUrl(this.router.url);
			if (languageFromUrl && language !== languageFromUrl) {
				this.languageFacade.updateLanguage(languageFromUrl);
			}
		});
	}

	private checkIfRouteIsChanged(): void {
		const {
			getQueue: sessionDcvIds,
			getLastDcv: lastSessionStoreItem,
			getSecondLastDcv: secondLastSessionStoreItem,
			getThirdLastDcv: thirdLastSessionStoreItem
		} = this.routeSessionStore;
		const { currentDcvIds } = this.routingService;
		const firstCurrentDcv = this.getIndexDcvIdfromQueue(currentDcvIds, 0);
		const secondCurrentDcv = this.getIndexDcvIdfromQueue(currentDcvIds, 1);

		// Initial application state
		if (
			!currentDcvIds ||
			currentDcvIds.length === 0 ||
			currentDcvIds.some((dcv) => !dcv)
		) {
			return;
		}

		if (
			currentDcvIds.length === 1 ||
			firstCurrentDcv === secondCurrentDcv
		) {
			// Go to Homepage
			this.routingFacade.updateQueue([firstCurrentDcv]);
		} else if (
			firstCurrentDcv === lastSessionStoreItem &&
			!secondLastSessionStoreItem
		) {
			// forward from homepage
			this.routingFacade.updateQueue([
				...sessionDcvIds,
				secondCurrentDcv
			]);
		} else if (
			firstCurrentDcv === lastSessionStoreItem &&
			secondLastSessionStoreItem
		) {
			// Native forward
			this.routingFacade.updateQueue([
				...sessionDcvIds,
				secondCurrentDcv
			]);
		} else if (
			firstCurrentDcv !== secondLastSessionStoreItem &&
			secondCurrentDcv === secondLastSessionStoreItem
		) {
			// Native back
			sessionDcvIds.pop();
			this.routingFacade.updateQueue([...sessionDcvIds]);
		} else if (firstCurrentDcv === secondLastSessionStoreItem) {
			// Left pane click, right screen change
			sessionDcvIds.pop();
			this.routingFacade.updateQueue([
				...sessionDcvIds,
				secondCurrentDcv
			]);
		}

		// if where we are going is the same as directly (previous in queue) in the past, then we pop last item and thus go backwards.
		if (
			thirdLastSessionStoreItem !== undefined &&
			lastSessionStoreItem === thirdLastSessionStoreItem &&
			secondLastSessionStoreItem === secondCurrentDcv
		) {
			sessionDcvIds.pop();
			this.routingFacade.updateQueue([...sessionDcvIds]);
		}
	}

	private getIndexDcvIdfromQueue(
		queue: string[],
		index: number
	): string | undefined {
		return queue && queue.length > index ? queue[index] : undefined;
	}
}
