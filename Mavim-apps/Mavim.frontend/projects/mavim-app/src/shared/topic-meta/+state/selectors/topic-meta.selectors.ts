import { createFeatureSelector, createSelector } from '@ngrx/store';
import { TopicMetaState } from '../../interfaces/topic-meta-state.interface';

export const selectTopicMetaState =
	createFeatureSelector<TopicMetaState>('topicMeta');

export const selectTypesLoaded = createSelector(
	selectTopicMetaState,
	(topicMetaState) => topicMetaState.TypesLoaded
);
export const selectTypes = createSelector(
	selectTopicMetaState,
	(topicMetaState) => topicMetaState.Types
);

export const selectIconsLoaded = createSelector(
	selectTopicMetaState,
	(topicMetaState) => topicMetaState.IconsLoaded
);
export const selectIcons = createSelector(
	selectTopicMetaState,
	(topicMetaState) => topicMetaState.Icons
);
