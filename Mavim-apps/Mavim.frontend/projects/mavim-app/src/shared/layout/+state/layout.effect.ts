import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { tap } from 'rxjs/operators';
import * as layoutActions from './layout.actions';
import { Favicons } from '../service';

@Injectable()
export class LayoutEffect {
	public constructor(
		private readonly actions$: Actions,
		private readonly favicons: Favicons
	) {}

	public layoutFavicon$ = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(layoutActions.LayoutActionTypes.favicon),
				tap((action: layoutActions.Favicon) => {
					this.favicons.activate(action.favicon);
				})
			);
		},
		{ dispatch: false }
	);
}
