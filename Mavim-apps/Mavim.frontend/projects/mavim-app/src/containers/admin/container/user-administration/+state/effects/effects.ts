import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { Observable, of } from 'rxjs';
import { Action } from '@ngrx/store';
import { map, catchError, mergeMap } from 'rxjs/operators';
import { AdminAuthorizationService } from '../../service/admin-user.service';
import {
	AdminActionTypes,
	SuccessLoadAuthorizedUsers,
	FailedLoadAuthorizedUsers,
	AddAuthorizedUsers,
	SuccessAddAuthorizedUsers,
	FailedAddAuthorizedUsers,
	EditAuthorizedUser,
	DeleteAuthorizedUser,
	SuccessEditAuthorizedUser,
	SuccessDeleteAuthorizedUser,
	FailedEditAuthorizedUser,
	FailedDeleteAuthorizedUser
} from '../actions/actions';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class AdminAuthorizedEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly userService: AdminAuthorizationService
	) {}

	public loadAuthorizedUsers$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(AdminActionTypes.LoadAuthorizedUsers),
			mergeMap(() =>
				this.userService.getAuthorizationUsers().pipe(
					map((u) => new SuccessLoadAuthorizedUsers(u)),
					catchError(() => of(new FailedLoadAuthorizedUsers()))
				)
			)
		);
	});

	public addAuthorizedUsers$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(AdminActionTypes.AddAuthorizedUsers),
			map((action: AddAuthorizedUsers) => action.payload),
			mergeMap((payload) =>
				this.userService.addAuthorizationUsers(payload).pipe(
					map((u) => new SuccessAddAuthorizedUsers(u)),
					catchError((response: HttpErrorResponse) =>
						of(
							new FailedAddAuthorizedUsers(
								(response.error?.error as string) ||
									(response.error as string)
							)
						)
					)
				)
			)
		);
	});

	public patchAuthorizedUsers$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(AdminActionTypes.EditAuthorizedUser),
			map((action: EditAuthorizedUser) => action.payload),
			mergeMap((payload) =>
				this.userService.patchAuthorizationUsers(payload).pipe(
					map(() => new SuccessEditAuthorizedUser(payload)),
					catchError((response: HttpErrorResponse) =>
						of(
							new FailedEditAuthorizedUser(
								(response.error?.error as string) ||
									(response.error as string)
							)
						)
					)
				)
			)
		);
	});

	public deleteAuthorizedUsers$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(AdminActionTypes.DeleteAuthorizedUser),
			map((action: DeleteAuthorizedUser) => action.payload),
			mergeMap((payload) =>
				this.userService.deleteAuthorizationUsers(payload).pipe(
					map(() => new SuccessDeleteAuthorizedUser(payload)),
					catchError((response: HttpErrorResponse) =>
						of(
							new FailedDeleteAuthorizedUser(
								(response.error?.error as string) ||
									(response.error as string)
							)
						)
					)
				)
			)
		);
	});
}
