import { SplitScreenState } from '../../interfaces/splitscreen-state.interface';
import { SplitScreenActions } from '../actions/splitscreen.actions';
import { ScreenState } from '../../enums/screenState';
import { SplitScreenActionTypes } from '../actions/splitscreen.actions';

export const initialSplitScreenState: SplitScreenState = {
	screenState: ScreenState.LeftScreenMaximized,
	treePanelVisible: false,
	sidebarVisible: false
};

export function splitScreenReducer(
	state: SplitScreenState = initialSplitScreenState,
	action: SplitScreenActions
): SplitScreenState {
	switch (action.type) {
		case SplitScreenActionTypes.SetScreenState: {
			const { screenState: currentScreenState } = state;
			const { payload: newScreenState } = action;
			return {
				...state,
				screenState: togglePanelState(
					currentScreenState,
					newScreenState
				)
			};
		}
		case SplitScreenActionTypes.ToggleTreePanelVisibility: {
			return { ...state, treePanelVisible: !state.treePanelVisible };
		}
		case SplitScreenActionTypes.ToggleSidebarVisibility: {
			return { ...state, sidebarVisible: !state.sidebarVisible };
		}
		default:
			return state;
	}
}

const togglePanelState = (
	currentScreenState: ScreenState,
	requestorScreenState: ScreenState
): ScreenState =>
	requestorScreenState === currentScreenState
		? ScreenState.SplitScreen
		: requestorScreenState;
