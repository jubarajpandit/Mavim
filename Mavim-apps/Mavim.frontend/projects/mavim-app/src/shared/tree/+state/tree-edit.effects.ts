import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { Action } from '@ngrx/store';
import * as treeEditActions from './tree-edit.actions';
import { TreeService } from '../services/tree.service';

@Injectable()
export class TreeEditEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly treeService: TreeService
	) {}

	public moveToTop$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(treeEditActions.MoveToTop),
			mergeMap((action) =>
				this.treeService.moveToTop(action.payload.topicId).pipe(
					map(() =>
						treeEditActions.MoveToTopSuccess({
							payload: {
								namespace: action.payload.namespace,
								topicId: action.payload.topicId
							}
						})
					),
					catchError(() => of(treeEditActions.MoveFailed()))
				)
			)
		);
	});

	public moveToBottom$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(treeEditActions.MoveToBottom),
			mergeMap((action) =>
				this.treeService.moveToBottom(action.payload.topicId).pipe(
					map(() =>
						treeEditActions.MoveToBottomSuccess({
							payload: {
								namespace: action.payload.namespace,
								topicId: action.payload.topicId
							}
						})
					),
					catchError(() => of(treeEditActions.MoveFailed()))
				)
			)
		);
	});

	public moveUp$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(treeEditActions.MoveUp),
			mergeMap((action) =>
				this.treeService.moveUp(action.payload.topicId).pipe(
					map(() =>
						treeEditActions.MoveUpSuccess({
							payload: {
								namespace: action.payload.namespace,
								topicId: action.payload.topicId
							}
						})
					),
					catchError(() => of(treeEditActions.MoveFailed()))
				)
			)
		);
	});

	public moveDown$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(treeEditActions.MoveDown),
			mergeMap((action) =>
				this.treeService.moveDown(action.payload.topicId).pipe(
					map(() =>
						treeEditActions.MoveDownSuccess({
							payload: {
								namespace: action.payload.namespace,
								topicId: action.payload.topicId
							}
						})
					),
					catchError(() => of(treeEditActions.MoveFailed()))
				)
			)
		);
	});

	public moveLevelUp$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(treeEditActions.MoveLevelUp),
			mergeMap((action) =>
				this.treeService.moveLevelUp(action.payload.topicId).pipe(
					map(() =>
						treeEditActions.MoveLevelUpSuccess({
							payload: {
								namespace: action.payload.namespace,
								topicId: action.payload.topicId
							}
						})
					),
					catchError(() => of(treeEditActions.MoveFailed()))
				)
			)
		);
	});

	public moveLevelDown$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(treeEditActions.MoveLevelDown),
			mergeMap((action) =>
				this.treeService.moveLevelDown(action.payload.topicId).pipe(
					map(() =>
						treeEditActions.MoveLevelDownSuccess({
							payload: {
								namespace: action.payload.namespace,
								topicId: action.payload.topicId
							}
						})
					),
					catchError(() => of(treeEditActions.MoveFailed()))
				)
			)
		);
	});
}
