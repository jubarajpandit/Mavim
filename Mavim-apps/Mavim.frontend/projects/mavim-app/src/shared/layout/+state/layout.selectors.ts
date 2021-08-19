import { createFeatureSelector, createSelector } from '@ngrx/store';
import { LayoutState } from '.';

export const selectLayoutState = createFeatureSelector<LayoutState>('layout');

export const selectFullScreenState = createSelector(
	selectLayoutState,
	(layoutState) => layoutState.fullscreen
);

export const selectSettingsMenuState = createSelector(
	selectLayoutState,
	(layoutState) => layoutState.settingsMenu
);
