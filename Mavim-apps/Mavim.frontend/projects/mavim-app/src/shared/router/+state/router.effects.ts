import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { tap, take } from 'rxjs/operators';
import { RoutingSessionStoreService, RoutingService } from '../service';
import * as routingActions from './routing.actions';
import { LanguageFacade } from '../../language/service/language.facade';

@Injectable()
export class RoutingEffect {
	public constructor(
		private readonly actions$: Actions,
		private readonly routing: RoutingService,
		private readonly languageFacade: LanguageFacade,
		private readonly sessionStoreService: RoutingSessionStoreService
	) {}

	public routingHome$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.Home),
				tap((action) => {
					this.languageFacade.language
						.pipe(take(1))
						.subscribe((language) => {
							this.routing.start(language, action.payload);
						});
				})
			);
		},
		{ dispatch: false }
	);

	public routingNext$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.Next),
				tap((action) => {
					this.languageFacade.language
						.pipe(take(1))
						.subscribe((language) => {
							this.routing.next(
								language,
								action.payload.dcvId,
								action.payload.outlet
							);
						});
				})
			);
		},
		{ dispatch: false }
	);

	public routingBack$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.Back),
				tap(() => {
					this.languageFacade.language
						.pipe(take(1))
						.subscribe((language) => {
							this.routing.back(language);
						});
				})
			);
		},
		{ dispatch: false }
	);

	public routingEdit$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.Edit),
				tap((action) => {
					this.languageFacade.language
						.pipe(take(1))
						.subscribe((language) => {
							this.routing.navigate([
								language,
								this.editPage,
								action.payload
							]);
						});
				})
			);
		},
		{ dispatch: false }
	);

	public routingWordEdit$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.EditWord),
				tap((action) => {
					this.languageFacade.language
						.pipe(take(1))
						.subscribe((language) => {
							this.routing.navigateNewTab(
								`${language}/${this.editPage}/${this.wordPage}/${action.payload}`
							);
						});
				})
			);
		},
		{ dispatch: false }
	);

	public routingTestWopi$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.TestWopi),
				tap((action) => {
					this.routing.navigateNewTab(
						`${this.wopiTestPage}/${action.payload}`
					);
				})
			);
		},
		{ dispatch: false }
	);

	public routingWordAdd$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.CreateNewWord),
				tap((action) => {
					this.languageFacade.language
						.pipe(take(1))
						.subscribe((language) => {
							this.routing.navigateNewTab(
								`${language}/${this.newPage}/${this.wordPage}/${action.payload.dcv}`
							);
						});
				})
			);
		},
		{ dispatch: false }
	);

	public routingEditUsers$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.EditUsers),
				tap(() => {
					this.routing.navigate([this.adminPage, this.usersPage]);
				})
			);
		},
		{ dispatch: false }
	);

	public routingEditBack$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.BackEdit),
				tap(() => {
					this.languageFacade.language
						.pipe(take(1))
						.subscribe((language) => {
							this.routing.backEdit(language);
						});
				})
			);
		},
		{ dispatch: false }
	);

	public routingInit$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.Init),
				tap((action) => {
					this.sessionStoreService.setQueue(action.payload);
				})
			);
		},
		{ dispatch: false }
	);

	public routingUpdateQueue$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(routingActions.UpdateQueue),
				tap((action) => {
					this.sessionStoreService.setQueue(action.payload);
				})
			);
		},
		{ dispatch: false }
	);

	private readonly editPage = 'edit';
	private readonly adminPage = 'admin';
	private readonly usersPage = 'users';
	private readonly wopiTestPage = 'test';
	private readonly wordPage = 'word';
	private readonly newPage = 'new';
}
