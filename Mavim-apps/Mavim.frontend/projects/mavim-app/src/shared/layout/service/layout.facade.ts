import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import * as actions from '../+state/layout.actions';
import {
	selectFullScreenState,
	selectSettingsMenuState
} from '../+state/layout.selectors';
import { Icons } from '../types';

@Injectable()
export class LayoutFacade {
	public constructor(private readonly store: Store) {}

	public get layoutState(): Observable<boolean | undefined> {
		return this.store.select(selectFullScreenState);
	}

	public get settingsMenu(): Observable<boolean> {
		return this.store.select(selectSettingsMenuState);
	}

	public setFullscreen(isFullscreen: boolean): void {
		this.store.dispatch(new actions.Fullscreen(isFullscreen));
	}

	public setFavicon(favicon: Icons): void {
		this.store.dispatch(new actions.Favicon(favicon));
	}

	public toggleSettingsMenu(): void {
		this.store.dispatch(new actions.ToggleSettingsMenu());
	}
}
