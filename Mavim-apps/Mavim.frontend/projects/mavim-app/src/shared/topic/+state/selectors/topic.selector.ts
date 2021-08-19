import { TopicState } from '../../interfaces/topic-state.interface';
import {
	createFeatureSelector,
	createSelector,
	DefaultProjectorFn,
	MemoizedSelector
} from '@ngrx/store';
import { Topic } from '../../models/topic.model';
import { ElementType } from '../../enums/element-type.enum';

export const selectTopicsState = createFeatureSelector<TopicState>('topics');

export const selectTopicById = (
	dcv: string
): MemoizedSelector<TopicState, Topic, DefaultProjectorFn<Topic>> =>
	createSelector(
		selectTopicsState,
		(topicsState) => topicsState.entities[dcv]
	);

export const selectRootElement = createSelector(
	selectTopicsState,
	(topicsState) =>
		Object.values(topicsState.entities).find(
			(topic) => topic.typeCategory === ElementType.MavimElementContainer
		)
);

export const selectRecycleBin = createSelector(
	selectTopicsState,
	(topicsState) =>
		Object.values(topicsState.entities).find(
			(topic) => topic.typeCategory === ElementType.RecycleBin
		)
);

export const selectCategories = createSelector(
	selectTopicsState,
	(topicsState) =>
		Object.values(topicsState.entities).filter(
			(categoriesTopic: Topic) =>
				categoriesTopic.parent ===
				Object.values(topicsState.entities).find(
					(topic) =>
						topic.typeCategory === ElementType.RelationCategories
				).dcv
		)
);

export const topicLoaded = createSelector(
	selectTopicsState,
	(topicsState) => topicsState.topicLoaded
);

export const lastTopicUpdated = createSelector(
	selectTopicsState,
	(topicsState) => topicsState.lastTopicUpdated
);

export const allTopicsLoaded = createSelector(
	selectTopicsState,
	(topicsState) => topicsState.allTopicsLoaded
);

export const selectChildTopicsByDcv = (
	dcv: string
): MemoizedSelector<TopicState, Topic[], DefaultProjectorFn<Topic[]>> =>
	createSelector(selectTopicsState, (topicsState) => {
		const allTopics = Object.values(topicsState.entities);
		return topicsState.topicLoaded
			? allTopics
					.filter((topic: Topic) => topic.parent === dcv)
					.sort(orderByNumber)
			: undefined;
	});

export const selectEditTopic = createSelector(
	selectTopicsState,
	(topicsState) => topicsState.lastTopicUpdated
);

const orderByNumber = (a: Topic, b: Topic): number => {
	if (a.orderNumber < b.orderNumber) {
		return -1;
	}
	if (a.orderNumber > b.orderNumber) {
		return 1;
	}
	return 0;
};
