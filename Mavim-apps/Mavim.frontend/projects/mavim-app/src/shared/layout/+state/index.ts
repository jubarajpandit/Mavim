import { Icons } from '../types';

export * from './layout.reducer';
export * from './layout.actions';
export * from './layout.effect';

export class LayoutState {
	public fullscreen: boolean | undefined;
	public favicon: Icons | undefined;
	public settingsMenu: boolean;
}
