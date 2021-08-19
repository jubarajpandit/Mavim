import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { FeatureflagService } from '../../service/featureflag.service';
import * as FeatureFlagActions from '../actions/featureflag.actions';

@Injectable({
	providedIn: 'root'
})
export class FeatureflagEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly service: FeatureflagService
	) {}

	public actionName$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(FeatureFlagActions.featurefagRequest),
			mergeMap(() =>
				this.service.getAuthorizationRights().pipe(
					map((featureflags) =>
						FeatureFlagActions.featurefagSuccess({
							payload: featureflags
						})
					),
					catchError(() => of(FeatureFlagActions.featurefagFailure()))
				)
			)
		);
	});
}
