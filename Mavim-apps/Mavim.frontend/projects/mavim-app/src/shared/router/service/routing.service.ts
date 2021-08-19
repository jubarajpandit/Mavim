import { Injectable, Inject } from '@angular/core';
import {
	ActivatedRoute,
	Router,
	ActivatedRouteSnapshot
} from '@angular/router';
import { Outlet } from '../models/outlet';
import { RoutingSessionStoreService } from './routing-session-store.service';
import { WINDOW } from './window.provider';
import { Language } from '../../language/enums/language.enum';

@Injectable()
export class RoutingService {
	public constructor(
		private readonly route: ActivatedRoute,
		private readonly router: Router,
		private readonly routeSessionStore: RoutingSessionStoreService,
		@Inject(WINDOW) private readonly window: Window
	) {}

	private readonly leftOutlet = 'left';
	private readonly rightOutlet = 'right';
	private readonly topicPage = 'topic';
	private readonly splitscreenPage = 'nav';

	private readonly dcvID = 'dcvid';
	private readonly languageComponent = 0;
	private readonly primaryComponent = 0;
	private readonly splitScreenComponent = 0;

	private get currentChildren(): ActivatedRouteSnapshot[] {
		return this.route.snapshot.children[this.languageComponent]?.children[
			this.primaryComponent
		]?.children[this.splitScreenComponent]?.children;
	}

	/**
	 * Gets the current dvc ids
	 */
	public get currentDcvIds(): string[] {
		const params = this.currentChildren?.map(
			(child) => child.params[this.dcvID] as string
		);
		return params;
	}

	/**
	 * Go to Start screen
	 */
	public start(language: Language, dcvID: string): void {
		if (dcvID) {
			this.navigateSplitScreen(language, dcvID, undefined);
		} else {
			this.router.navigate(['']);
		}
	}

	/**
	 * Display next screen in right outlet
	 * Copy (old) right outlet and shift to left
	 */
	public next(language: Language, dcvID: string, outlet: Outlet): void {
		const rightDcvId = this.getCurrentOutletDcvId(outlet);
		if (dcvID !== rightDcvId) {
			this.navigateSplitScreen(language, rightDcvId, dcvID);
		}
	}

	/**
	 * Display the previous screen in the left and right outlet
	 * Shifts the screens to the right
	 */
	public back(language: Language): void {
		const { getSecondLastDcv, getThirdLastDcv } = this.routeSessionStore;
		if (getThirdLastDcv) {
			this.navigateSplitScreen(
				language,
				getThirdLastDcv,
				getSecondLastDcv
			);
		} else {
			this.start(language, getSecondLastDcv);
		}
	}

	/**
	 * Go back to splitscreen from Edit page
	 */
	public backEdit(language: Language): void {
		const { getLastDcv, getSecondLastDcv } = this.routeSessionStore;
		if (getSecondLastDcv) {
			this.navigateSplitScreen(language, getSecondLastDcv, getLastDcv);
		} else {
			this.start(language, getLastDcv);
		}
	}

	/**
	 * navigate to path
	 */
	public navigate(path: string[]): void {
		this.router.navigate(path);
	}

	/**
	 * navigate to path in a new tab
	 */
	public navigateNewTab(path: string): void {
		window.open(`${this.window.location.origin}/${path}`, '_blank');
	}

	/**
	 * navigate to current path, for example to update the language
	 */
	public navigateCurrentPath(language: Language): void {
		const { getLastDcv, getSecondLastDcv } = this.routeSessionStore;
		if (getSecondLastDcv) {
			this.navigateSplitScreen(language, getSecondLastDcv, getLastDcv);
		} else {
			this.navigateSplitScreen(language, getLastDcv, undefined);
		}
	}

	private getCurrentOutletDcvId(outlet: Outlet): string {
		const outletData = this.currentChildren.find(
			(child) => child.outlet === outlet
		);
		return outletData
			? (outletData.params[this.dcvID] as string)
			: undefined;
	}

	private navigateSplitScreen(
		language: Language,
		leftDcvID: string,
		rightDcvID: string
	): void {
		const outlets = {};
		if (leftDcvID) {
			outlets[this.leftOutlet] = [this.topicPage, leftDcvID];
		}

		outlets[this.rightOutlet] = rightDcvID
			? [this.topicPage, rightDcvID]
			: null;

		if (leftDcvID || rightDcvID) {
			this.router.navigate([language, this.splitscreenPage, { outlets }]);
		}
	}
}
