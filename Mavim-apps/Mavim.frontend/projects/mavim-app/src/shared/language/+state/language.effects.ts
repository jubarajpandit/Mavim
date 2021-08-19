import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { tap } from 'rxjs/operators';
import * as languageActions from './language.actions';
import { LanguageSessionStoreService } from '../service/language-session-store.service';
import { RoutingService } from '../../router/service';
import { TopicFacade } from '../../topic/services/topic.facade';

@Injectable()
export class LanguageEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly sessionStoreService: LanguageSessionStoreService,
		private readonly topicFacade: TopicFacade,
		private readonly routingService: RoutingService
	) {}

	public setInitialDatabaseLanguage$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(languageActions.InitialLanguage),
				tap((action) => {
					this.sessionStoreService.setDatabaseLanguage(
						action.payload
					);
				})
			);
		},
		{ dispatch: false }
	);

	public updateDatabaseLanguage$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(languageActions.UpdateLanguage),
				tap((action) => {
					this.sessionStoreService.setDatabaseLanguage(
						action.payload
					);
				})
			);
		},
		{ dispatch: false }
	);

	public navigateToCurrentPage$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(languageActions.UpdateLanguage),
				tap((action) => {
					this.routingService.navigateCurrentPath(action.payload);
					this.topicFacade.loadRoot();
					this.topicFacade.loadCategories();
				})
			);
		},
		{ dispatch: false }
	);
}
