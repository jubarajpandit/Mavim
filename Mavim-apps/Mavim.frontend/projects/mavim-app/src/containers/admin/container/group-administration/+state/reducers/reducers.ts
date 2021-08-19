import { createReducer, on } from '@ngrx/store';
import { FetchStatus } from 'projects/mavim-app/src/shared/enums/FetchState';
import { Group } from '../../model/group';
import * as GroupsDetailsAction from '../actions/actions';

export interface State {
	groups: Group[];
	fetchStatus: FetchStatus;
}

const initialState: State = {
	groups: [],
	fetchStatus: FetchStatus.NotFetched
};

export const reducer = createReducer(
	initialState,
	on(
		GroupsDetailsAction.loadGroups,
		(state): State => ({
			...state,
			fetchStatus: FetchStatus.Loading
		})
	),
	on(
		GroupsDetailsAction.loadGroupsSuccess,
		(state, { payload }): State => ({
			...state,
			groups: payload,
			fetchStatus: FetchStatus.Fetched
		})
	),
	on(
		GroupsDetailsAction.loadGroupsFailed,
		(state): State => ({
			...state,
			fetchStatus: FetchStatus.Fetched
		})
	)
);
