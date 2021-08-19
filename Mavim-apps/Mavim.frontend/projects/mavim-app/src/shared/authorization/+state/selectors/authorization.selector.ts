import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AuthorizationState } from '../../interfaces/authorization-state.interface';

export const selectAuthorizationState =
	createFeatureSelector<AuthorizationState>('authorization');

export const selectEditTopic = createSelector(
	selectAuthorizationState,
	(authorizationState) => authorizationState.account
);
