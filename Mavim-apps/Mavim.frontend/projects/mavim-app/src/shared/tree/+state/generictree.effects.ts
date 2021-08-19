import { ofType, Actions, createEffect } from '@ngrx/effects';
import { TreeService } from '../services/tree.service';
import * as TreeActions from './generictree.actions';
import { map, mergeMap, catchError, filter } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { Action } from '@ngrx/store';
import * as TopicActions from '../../topic/+state/actions/topic.actions';
import { delay } from 'rxjs/operators';

export abstract class TreeEffectBase {
	public constructor(
		protected readonly actions$: Actions,
		protected readonly treeService: TreeService,
		protected readonly namespace: string,
		private readonly removeStatusDelay = 1000
	) {}

	public expandToNode: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(TreeActions.ExpandToNode),
			filter((action) => action.payload.namespace === this.namespace),
			mergeMap((action) =>
				this.treeService.getPathToTopic(action.payload.topicId).pipe(
					map((path) =>
						TreeActions.ExpandToNodeSuccess({
							payload: {
								namespace: this.namespace,
								path
							}
						})
					),
					catchError(() =>
						of(
							TreeActions.ExpandToNodeFailed({
								payload: {
									namespace: this.namespace
								}
							})
						)
					)
				)
			)
		);
	});

	public removeCreateStatus: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(
				TopicActions.CreateTopicSuccess,
				TopicActions.CreateChildTopicSuccess
			),
			delay(this.removeStatusDelay),
			map((action) =>
				TreeActions.RemoveCreateState({
					payload: {
						namespace: this.namespace,
						topicId: action.payload.topic.dcv
					}
				})
			)
		);
	});
}
