import { ScreenState } from '../enums/screenState';

export interface SplitScreenState {
	screenState: ScreenState;
	treePanelVisible: boolean;
	sidebarVisible: boolean;
}
