import { createReducer, on } from '@ngrx/store';
import { FetchStatus } from '../../../enums/FetchState';
import * as FeatureflagAction from '../actions/featureflag.actions';

export interface State {
	featureflags: string[];
	fetchStatus: FetchStatus;
}

const initialState: State = {
	featureflags: [],
	fetchStatus: FetchStatus.NotFetched
};

export const reducer = createReducer(
	initialState,
	on(
		FeatureflagAction.featurefagRequest,
		(state): State => ({
			...state,
			fetchStatus: FetchStatus.Loading
		})
	),
	on(
		FeatureflagAction.featurefagSuccess,
		(state, { payload }): State => ({
			...state,
			featureflags: payload || [],
			fetchStatus: FetchStatus.Fetched
		})
	),
	on(
		FeatureflagAction.featurefagFailure,
		(state): State => ({
			...state,
			fetchStatus: FetchStatus.Fetched
		})
	)
);
