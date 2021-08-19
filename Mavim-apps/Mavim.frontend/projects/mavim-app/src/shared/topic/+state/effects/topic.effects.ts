import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import * as topicActions from '../actions/topic.actions';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { TopicService } from '../../services/topic.service';
import { Observable, of } from 'rxjs';
import { Action } from '@ngrx/store';
import { ElementType } from '../../enums/element-type.enum';
import { Topic } from '../../models/topic.model';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class TopicEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly topicService: TopicService
	) {}

	public loadTopicChildren$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicActions.LoadTopicChildren),
			mergeMap((action) =>
				this.topicService.getChildTopics(action.payload).pipe(
					map((children) =>
						topicActions.LoadTopicChildrenSuccess({
							payload: this.enrichListWithHttpStatusCode(
								children,
								this.httpSuccessCode
							)
						})
					),
					catchError(() => of(topicActions.LoadTopicFail()))
				)
			)
		);
	});

	public loadTopicRoot$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicActions.LoadTopicRoot),
			mergeMap(() =>
				this.topicService.getTreeRoot().pipe(
					map((topic) =>
						topicActions.LoadTopicRootSuccess({
							payload: this.enrichWithHttpStatusCode(
								topic,
								this.httpSuccessCode
							)
						})
					),
					catchError(() => of(topicActions.LoadTopicFail()))
				)
			)
		);
	});

	public loadTopicSiblings$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(
				topicActions.LoadTopicSuccess,
				topicActions.LoadTopicRootSuccess
			),
			mergeMap((action) =>
				this.topicService.getTopicSiblings(action.payload.dcv).pipe(
					map((siblings) =>
						topicActions.LoadTopicSiblingsSuccess({
							payload: this.enrichListWithHttpStatusCode(
								siblings,
								this.httpSuccessCode
							)
						})
					),
					catchError(() => of(topicActions.LoadTopicFail()))
				)
			)
		);
	});

	public loadTopicCategoriesSiblings$: Observable<Action> = createEffect(
		() => {
			return this.actions$.pipe(
				ofType(topicActions.LoadRelationshipSuccess),
				mergeMap((action) => {
					const parentRelationshipCategory = action.payload.find(
						(t) => t.typeCategory === ElementType.RelationCategories
					);
					return this.topicService
						.getTopicSiblings(parentRelationshipCategory.dcv)
						.pipe(
							map((siblings) =>
								topicActions.LoadTopicSiblingsSuccess({
									payload: this.enrichListWithHttpStatusCode(
										siblings,
										this.httpSuccessCode
									)
								})
							),
							catchError(() => of(topicActions.LoadTopicFail()))
						);
				})
			);
		}
	);

	public loadTopicByDcv$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicActions.LoadTopicByDCV),
			mergeMap((action) =>
				this.topicService.getTopicByDcv(action.payload).pipe(
					map((topic) =>
						topicActions.LoadTopicSuccess({
							payload: this.enrichWithHttpStatusCode(
								topic,
								this.httpSuccessCode
							)
						})
					),
					catchError((error: HttpErrorResponse) =>
						of(
							topicActions.LoadTopicFailWithStatusCode({
								payload: {
									dcv: action.payload,
									resources: [],
									httpStatusCode: error.status
								} as Topic
							})
						)
					)
				)
			)
		);
	});

	public loadRelationshipTypes$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicActions.LoadRelationshipTypes),
			mergeMap(() =>
				this.topicService.getRelationshipTypes().pipe(
					map((relationshipTopics) =>
						topicActions.LoadRelationshipSuccess({
							payload: this.enrichListWithHttpStatusCode(
								relationshipTopics,
								this.httpSuccessCode
							)
						})
					),
					catchError(() => of(topicActions.LoadTopicFail()))
				)
			)
		);
	});

	public updateTopicName$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicActions.UpdateTopicName),
			mergeMap((action) =>
				this.topicService.updateTopicName(action.payload).pipe(
					map((topic) =>
						topicActions.UpdateTopicNameSuccess({
							payload: this.enrichWithHttpStatusCode(
								topic,
								this.httpSuccessCode
							)
						})
					),
					catchError(() => of(topicActions.LoadTopicFail()))
				)
			)
		);
	});

	public createTopic$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicActions.CreateTopic),
			mergeMap((action) => {
				const { topicId, name, type, icon } = action.payload;
				return this.topicService
					.createTopicById(topicId, name, type, icon)
					.pipe(
						map((topic) =>
							topicActions.CreateTopicSuccess({
								payload: {
									topic: this.enrichWithHttpStatusCode(
										topic,
										this.httpSuccessCode
									),
									topicAbove: topicId
								}
							})
						),
						catchError(() => of(topicActions.CreateTopicFailure()))
					);
			})
		);
	});

	public createChildTopic$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicActions.CreateChildTopic),
			mergeMap((action) => {
				const { parentTopicId, name, type, icon } = action.payload;
				return this.topicService
					.createChildTopicById(parentTopicId, name, type, icon)
					.pipe(
						map((topic) =>
							topicActions.CreateChildTopicSuccess({
								payload: {
									parentTopicId,
									topic: this.enrichWithHttpStatusCode(
										topic,
										this.httpSuccessCode
									)
								}
							})
						),
						catchError(() =>
							of(
								topicActions.CreateChildTopicFailure({
									payload: parentTopicId
								})
							)
						)
					);
			})
		);
	});

	public deleteTopic$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(topicActions.DeleteTopic),
			mergeMap((action) => {
				return this.topicService
					.deleteTopicById(action.payload.topicId)
					.pipe(
						map(() =>
							topicActions.DeleteTopicSuccess({
								payload: action.payload
							})
						),
						catchError(() =>
							of(
								topicActions.DeleteTopicFailure({
									payload: action.payload.topicId
								})
							)
						)
					);
			})
		);
	});

	private readonly httpSuccessCode = 200;

	private enrichListWithHttpStatusCode(
		topics: Topic[],
		httpStatusCode: number
	): Topic[] {
		return topics?.map((topic: Topic) =>
			this.enrichWithHttpStatusCode(topic, httpStatusCode)
		);
	}

	private enrichWithHttpStatusCode(
		topic: Topic,
		httpStatusCode: number
	): Topic {
		return { ...topic, httpStatusCode };
	}
}
