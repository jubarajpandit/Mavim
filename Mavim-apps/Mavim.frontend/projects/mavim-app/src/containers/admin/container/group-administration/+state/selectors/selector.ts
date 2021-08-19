import { createFeatureSelector, createSelector } from '@ngrx/store';
import { State } from '../reducers/reducers';

const selectGroupsState = createFeatureSelector<State>('groups');

export const selectGroups = createSelector(
	selectGroupsState,
	(state) => state.groups
);

export const selectGroupFetchStatus = createSelector(
	selectGroupsState,
	(state) => state.fetchStatus
);
