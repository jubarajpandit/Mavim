import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { RelationService } from '../../services/relation.service';
import * as relationActions from '../actions/relation.actions';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { Relation } from '../../models/relation.model';
import { of } from 'rxjs';
import { Store } from '@ngrx/store';

@Injectable()
export class RelationEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly relationService: RelationService,
		private store: Store
	) {}

	public loadFields$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(relationActions.LoadRelations),
			mergeMap((action) =>
				this.relationService.getRelations(action.payload).pipe(
					map((relations: Relation[]) =>
						relationActions.LoadRelationsSuccess({
							payload: relations
						})
					),
					catchError(() => of(relationActions.LoadRelationsFail()))
				)
			)
		);
	});

	public createRelation$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(relationActions.CreateRelation),
			mergeMap((action) =>
				this.relationService.createRelation(action.payload).pipe(
					map((relation) =>
						relationActions.CreateRelationSuccess({
							payload: relation
						})
					),
					catchError(() => of(relationActions.CreateRelationFail()))
				)
			)
		);
	});

	public deleteRelation$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(relationActions.DeleteRelation),
			mergeMap((action) =>
				this.relationService.deleteRelation(action.payload).pipe(
					map(() => {
						const { topicDCV, dcv } = action.payload;
						return relationActions.DeleteRelationSuccess({
							payload: `${topicDCV}_${dcv}`
						});
					}),
					catchError(() => of(relationActions.DeleteRelationFail()))
				)
			)
		);
	});
}
