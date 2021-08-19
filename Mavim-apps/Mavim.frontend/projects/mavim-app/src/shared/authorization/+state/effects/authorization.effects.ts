import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { Observable, of } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';
import * as authorizationActions from '../actions/authentication.actions';
import { AuthorizationService } from '../../service/authorization.service';
import { Action } from '@ngrx/store';

@Injectable()
export class AuthorizationEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly authorizationService: AuthorizationService
	) {}

	public getReadOnly$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(
				authorizationActions.AuthorizationActionTypes.LoadAuthorization
			),
			mergeMap(() =>
				this.authorizationService.getAuthorizationRights().pipe(
					map(
						(authorization) =>
							new authorizationActions.LoadAuthorizationSuccess(
								authorization
							)
					),
					catchError(() =>
						of(new authorizationActions.LoadAuthorizationFailed())
					)
				)
			)
		);
	});
}
