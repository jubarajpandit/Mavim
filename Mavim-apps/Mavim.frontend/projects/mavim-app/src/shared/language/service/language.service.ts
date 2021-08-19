import { Injectable } from '@angular/core';
import { Language } from '../enums/language.enum';
import { LanguageFacade } from './language.facade';
import { Router, NavigationEnd, RouterEvent } from '@angular/router';
import { LanguageSessionStoreService } from './language-session-store.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LanguageService {
	public constructor(
		private readonly languageFacade: LanguageFacade,
		private readonly router: Router,
		private readonly sessionStoreService: LanguageSessionStoreService
	) {}

	private readonly defaultLanguage = Language.English;

	public initializeDatabaseLanguage(): void {
		const routerSubscription = new Subject();

		this.router.events
			.pipe(takeUntil(routerSubscription))
			.subscribe((e) => {
				const event = e as RouterEvent;
				const languageFromUrl = this.getLanguageFromUrl(event?.url);
				if (languageFromUrl || e instanceof NavigationEnd) {
					this.languageFacade.initialLanguage(
						languageFromUrl ??
							this.getLanguageFromSessionOrDefault()
					);
					routerSubscription.next();
					routerSubscription.complete();
				}
			});
	}

	public mapToLanguage(language: string): Language {
		switch (language) {
			case 'nl':
				return Language.Dutch;
			case 'en':
				return Language.English;
			default:
				return undefined;
		}
	}

	public getLanguageFromUrl(url: string): Language {
		return this.mapToLanguage(
			url?.split('/')?.find((x) => this.mapToLanguage(x) !== undefined)
		);
	}

	private getLanguageFromSessionOrDefault(): Language {
		const languageFromSession: Language = this.mapToLanguage(
			this.sessionStoreService.getDatabaseLanguage
		);
		const databaseLanguage: Language =
			languageFromSession ?? this.defaultLanguage;

		return databaseLanguage;
	}
}
