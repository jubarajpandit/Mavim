import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import * as topicMetaActions from '../actions/topic-meta.actions';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { TopicMetaService } from '../../services/topic-meta.service';

@Injectable()
export class TopicMetaEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly topicMetaService: TopicMetaService
	) {}

	public loadTopicTypes$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicMetaActions.LoadTopicTypes),
			mergeMap((action) =>
				this.topicMetaService.getTopicTypes(action.payload).pipe(
					map((types) =>
						topicMetaActions.LoadTopicTypesSuccess({
							payload: types
						})
					),
					catchError(() =>
						of(topicMetaActions.LoadTopicTypesFailed())
					)
				)
			)
		);
	});

	public loadTopicIcons$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicMetaActions.LoadTopicIcons),
			mergeMap((action) =>
				this.topicMetaService.getTopicIcons(action.payload).pipe(
					map((icons) =>
						topicMetaActions.LoadTopicIconsSuccess({
							payload: icons
						})
					),
					catchError(() =>
						of(topicMetaActions.LoadTopicIconsFailed())
					)
				)
			)
		);
	});
}
