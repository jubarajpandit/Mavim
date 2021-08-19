import { createSelector, createFeatureSelector } from '@ngrx/store';
import { SplitScreenState } from '../../interfaces/splitscreen-state.interface';

export const selectSplitScreenState =
	createFeatureSelector<SplitScreenState>('splitScreen');

export const SelectScreenState = createSelector(
	selectSplitScreenState,
	(splitScreen) => splitScreen.screenState
);

export const SelectTreePanelVisible = createSelector(
	selectSplitScreenState,
	(splitscreen) => splitscreen.treePanelVisible
);

export const SelectSidebarVisible = createSelector(
	selectSplitScreenState,
	(splitscreen) => splitscreen.sidebarVisible
);
