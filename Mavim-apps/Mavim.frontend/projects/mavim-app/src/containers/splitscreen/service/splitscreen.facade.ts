import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import * as fromSplitScreen from '../+state';
import { Store } from '@ngrx/store';
import { ScreenState } from '../enums/screenState';
import {
	SelectScreenState,
	SelectTreePanelVisible,
	SelectSidebarVisible
} from '../+state/selectors/splitscreen.selectors';

@Injectable()
export class SplitScreenFacade {
	public constructor(private readonly store: Store) {}

	public get screenState(): Observable<ScreenState> {
		return this.store.select(SelectScreenState);
	}

	public get treePanelVisibility(): Observable<boolean> {
		return this.store.select(SelectTreePanelVisible);
	}

	public get sidebarVisibility(): Observable<boolean> {
		return this.store.select(SelectSidebarVisible);
	}

	public setScreenState(side: ScreenState): void {
		this.store.dispatch(new fromSplitScreen.SetScreenState(side));
	}

	public toggleTreePanelVisibility(): void {
		this.store.dispatch(new fromSplitScreen.ToggleTreePanelVisibility());
	}

	public toggleSidebarVisibility(): void {
		this.store.dispatch(new fromSplitScreen.ToggleSidebarVisibility());
	}
}
