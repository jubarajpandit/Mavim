import { createReducer, on } from '@ngrx/store';
import { TopicMetaState } from '../../interfaces/topic-meta-state.interface';
import * as TopicMetaActions from '../actions/topic-meta.actions';

export const initialTopicMetaState: TopicMetaState = {
	IconsLoaded: true,
	TypesLoaded: true,
	Icons: {},
	Types: {}
};

export const topicMetaReducer = createReducer(
	initialTopicMetaState,
	on(
		TopicMetaActions.LoadTopicTypes,
		(state): TopicMetaState => ({
			...state,
			TypesLoaded: false
		})
	),
	on(
		TopicMetaActions.LoadTopicTypesSuccess,
		(state, { payload }): TopicMetaState => ({
			...state,
			Types: payload,
			TypesLoaded: true
		})
	),
	on(
		TopicMetaActions.LoadTopicTypesFailed,
		(state): TopicMetaState => ({
			...state,
			TypesLoaded: true
		})
	),
	on(
		TopicMetaActions.LoadTopicIcons,
		(state): TopicMetaState => ({
			...state,
			IconsLoaded: false
		})
	),
	on(
		TopicMetaActions.LoadTopicIconsSuccess,
		(state, { payload }): TopicMetaState => ({
			...state,
			Icons: payload,
			IconsLoaded: true
		})
	),
	on(
		TopicMetaActions.LoadTopicIconsFailed,
		(state): TopicMetaState => ({
			...state,
			IconsLoaded: true
		})
	),
	on(
		TopicMetaActions.ClearTopicMetaStore,
		(): TopicMetaState => ({
			...initialTopicMetaState
		})
	)
);
