import { Action } from '@ngrx/store';
import { Icons } from '../types';

export enum LayoutActionTypes {
	fullscreen = '[Layout] set fullscreen',
	favicon = '[Layout] set favicon',
	toggleSettingsMenu = '[Layout] toggle settings menu'
}

export class Fullscreen implements Action {
	public constructor(public fullscreen: boolean) {}
	public readonly type = LayoutActionTypes.fullscreen;
}

export class Favicon implements Action {
	public constructor(public favicon: Icons | undefined) {}
	public readonly type = LayoutActionTypes.favicon;
}

export class ToggleSettingsMenu implements Action {
	public readonly type = LayoutActionTypes.toggleSettingsMenu;
}

export type LayoutActions = Fullscreen | Favicon | ToggleSettingsMenu;
