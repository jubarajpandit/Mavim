import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { Observable, of } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';
import * as databaseActions from '../actions/database.actions';
import { DatabaseService } from '../../service/database.service';
import { Action } from '@ngrx/store';
import { DatabaseBrowserService } from '../../service/database-local-store.service';

@Injectable()
export class DatabaseEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly databaseService: DatabaseService,
		private readonly databaseBrowserService: DatabaseBrowserService
	) {}

	public getDatabases$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(databaseActions.DatabaseActionTypes.LoadDatabase),
			mergeMap(() =>
				this.databaseService.getDatabases().pipe(
					map((database) => {
						return new databaseActions.LoadDatabaseSuccess(
							database
						);
					}),
					catchError(() =>
						of(new databaseActions.LoadDatabaseFailed())
					)
				)
			)
		);
	});

	public getSelectedDatabaseFromStorage$: Observable<Action> = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(databaseActions.DatabaseActionTypes.InitDatabase),
				mergeMap(() =>
					this.databaseBrowserService.getSelectedDatabase.pipe(
						map((database) => {
							return new databaseActions.SetSelectedDatabase(
								database
							);
						})
					)
				)
			);
		}
	);
}
