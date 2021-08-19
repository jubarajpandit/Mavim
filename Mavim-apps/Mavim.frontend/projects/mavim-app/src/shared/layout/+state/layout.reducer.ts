import { LayoutState } from '.';
import { LayoutActions, LayoutActionTypes } from './layout.actions';

const initialRouterState: LayoutState = {
	fullscreen: undefined,
	favicon: undefined,
	settingsMenu: false
};

export function reducer(
	state: LayoutState = initialRouterState,
	action: LayoutActions
): LayoutState {
	switch (action.type) {
		case LayoutActionTypes.fullscreen: {
			const { fullscreen } = action;
			return { ...state, fullscreen };
		}
		case LayoutActionTypes.favicon: {
			const { favicon } = action;
			return { ...state, favicon };
		}
		case LayoutActionTypes.toggleSettingsMenu: {
			return { ...state, settingsMenu: !state.settingsMenu };
		}
		default: {
			return state;
		}
	}
}
