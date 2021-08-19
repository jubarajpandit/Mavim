import {
	createFeatureSelector,
	createSelector,
	DefaultProjectorFn,
	MemoizedSelector
} from '@ngrx/store';
import { State } from '../reducers/featureflag.reducers';

const selectfeatureflagState = createFeatureSelector<State>('featureflag');

export const selectFeatureflags = createSelector(
	selectfeatureflagState,
	(state) => state.featureflags
);

export const selectFetchFeatureflag = createSelector(
	selectfeatureflagState,
	(state) => state.fetchStatus
);

export const selectFeatureflag = (
	featureflag: string
): MemoizedSelector<State, boolean, DefaultProjectorFn<boolean>> =>
	createSelector(
		selectfeatureflagState,
		(state) => !!state.featureflags?.find((f) => f === featureflag)
	);
