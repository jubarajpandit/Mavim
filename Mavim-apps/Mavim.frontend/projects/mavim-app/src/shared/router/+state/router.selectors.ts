import { createFeatureSelector, createSelector } from '@ngrx/store';
import { RouterState } from '.';

export const selectRoutingState = createFeatureSelector<RouterState>('routing');

export const selectCurrentQueue = createSelector(
	selectRoutingState,
	(routingState) => routingState.queue
);
