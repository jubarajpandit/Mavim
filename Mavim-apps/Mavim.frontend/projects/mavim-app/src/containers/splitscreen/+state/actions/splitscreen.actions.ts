import { Action } from '@ngrx/store';
import { ScreenState } from '../../enums/screenState';

export enum SplitScreenActionTypes {
	SetScreenState = '[SplitScreen] Set Screen State',
	ToggleTreePanelVisibility = '[SplitScreen] Toggle Tree Panel Visibility',
	ToggleSidebarVisibility = '[SplitScreen] Toggle Sidebar Visibility'
}

// Action Creators
export class SetScreenState implements Action {
	public constructor(public payload: ScreenState) {}
	public readonly type = SplitScreenActionTypes.SetScreenState;
}

export class ToggleTreePanelVisibility implements Action {
	public readonly type = SplitScreenActionTypes.ToggleTreePanelVisibility;
}

export class ToggleSidebarVisibility implements Action {
	public readonly type = SplitScreenActionTypes.ToggleSidebarVisibility;
}

export type SplitScreenActions =
	| SetScreenState
	| ToggleTreePanelVisibility
	| ToggleSidebarVisibility;
