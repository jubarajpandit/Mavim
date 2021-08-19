import { Component, OnInit } from '@angular/core';
import { SplitScreenFacade } from '../../service/splitscreen.facade';
import { ScreenState } from '../../enums/screenState';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { takeUntil, map, take } from 'rxjs/operators';
import {
	RoutingFacade,
	RoutingBrowserService
} from '../../../../shared/router/service';
import { Observable, Subject } from 'rxjs';
import { TopicFacade } from '../../../../shared/topic/services/topic.facade';
import { routingQueryValues } from '../../../../shared/router/service/routing.constants';
import { FeatureflagFacade } from '../../../../shared/featureflag/service/featureflag.facade';
import { TooltipFeatures } from '../../../../shared/tooltip/enums/tooltipFeatures';

@Component({
	selector: 'mav-splitscreen',
	templateUrl: './splitscreen.component.html',
	styleUrls: ['./splitscreen.component.scss']
})
export class SplitScreenComponent implements OnInit {
	public tooltipFeatureFlag: boolean;
	public get isHomePage(): boolean {
		return this.route.snapshot.children.length === 1;
	}

	public get singleOutlet(): boolean {
		return this.route.children.length === 1;
	}

	public constructor(
		private readonly splitScreenFacade: SplitScreenFacade,
		private readonly routingFacade: RoutingFacade,
		private readonly route: ActivatedRoute,
		private readonly router: Router,
		private readonly featureflagFacade: FeatureflagFacade,
		private readonly topicFacade: TopicFacade,
		// do not remove the RoutingBrowserService, as it will not be subscribed to the route then.
		public readonly routingBrowserService: RoutingBrowserService
	) {}

	public readonly ScreenStateEnum = ScreenState; // This makes it posible to use enum in HTML
	public treePanelVisible = false;
	public screenState: ScreenState = ScreenState.LeftScreenMaximized;

	private readonly singleScreen = 1;
	private readonly dualScreen = 2;
	private rootElementDcvId: string; // Store root element
	private queue: string[] = [];

	public ngOnInit(): void {
		this.initFeatureflags();
		this.initRedirectToHomePage();
		this.initSetScreenStateByRoute();
		this.setTreeVisibility();
		this.setTreeVisibilityByScreenState();
		this.setScreenStateByRouteChange();
		this.setRedirectToHomePageByRouteChange();
		this.setPanelState();
		this.setScreenStateByTreeVisibility();
		this.getQueue();
		this.handleQueryParams();
	}

	public get sidebarVisibleWithoutTree(): Observable<boolean> {
		return this.routingFacade.queue.pipe(
			map(
				(history) =>
					history &&
					(history.length > this.dualScreen ||
						(history.length > this.singleScreen &&
							this.screenState ===
								ScreenState.RightScreenMaximized))
			)
		);
	}

	/**
	 * Opens next page when tree item is selected
	 */
	public nextPage(dcv: string): void {
		this.routingFacade.next(dcv, 'right');
	}

	/**
	 * handles the topic selected event from the tree component
	 */
	public handleTopicSelected(dcvId: string): void {
		this.nextPage(dcvId);
	}

	/**
	 * Toggle TreePanel visibility
	 */
	public toggleTreePanelVisibility(): void {
		this.splitScreenFacade.toggleTreePanelVisibility();
	}

	/**
	 * Sets screenstate
	 */
	public resize(state: ScreenState): void {
		this.splitScreenFacade.setScreenState(state);
	}

	/**
	 * Get queue
	 */
	private getQueue(): void {
		this.routingFacade.queue.subscribe((queue) => {
			this.queue = queue;
		});
	}

	private setTreeVisibility(): void {
		this.splitScreenFacade.treePanelVisibility.subscribe(
			(treePanelVisible: boolean) => {
				this.treePanelVisible = treePanelVisible;
			}
		);
	}

	private setTreeVisibilityByScreenState(): void {
		this.splitScreenFacade.screenState.subscribe((state: ScreenState) => {
			if (
				state === ScreenState.RightScreenMaximized &&
				this.treePanelVisible
			) {
				this.splitScreenFacade.toggleTreePanelVisibility();
			}
		});
	}

	/**
	 * set ScreenState by Router events
	 */
	private setScreenStateByRouteChange(): void {
		this.router.events.subscribe((e) => {
			if (e instanceof NavigationEnd) {
				this.setScreenStateByRoute();
			}
		});
	}

	/**
	 * Redirect to Homepage if url change to is /nav
	 */
	private setRedirectToHomePageByRouteChange(): void {
		this.router.events.subscribe((e) => {
			if (e instanceof NavigationEnd && e.url === '/nav') {
				this.redirectToRootElement();
			}
		});
	}

	/**
	 * Subscribe on the store to change ScreenState
	 */
	private setPanelState(): void {
		this.splitScreenFacade.screenState.subscribe((state: ScreenState) => {
			this.screenState = state;
		});
	}

	/**
	 * Set the correct screen state whenever the tree is opened
	 */
	private setScreenStateByTreeVisibility(): void {
		this.splitScreenFacade.treePanelVisibility.subscribe(
			(treePanelVisible: boolean) => {
				if (!treePanelVisible || this.queue.length <= 1) {
					return;
				}

				if (this.screenState === ScreenState.RightScreenMaximized) {
					this.splitScreenFacade.setScreenState(
						ScreenState.SplitScreen
					);
					return;
				}

				if (this.screenState === ScreenState.LeftScreenMaximized) {
					this.splitScreenFacade.setScreenState(
						ScreenState.SplitScreen
					);
					this.routingFacade.back();
					return;
				}
			}
		);
	}

	/**
	 * Initialize homepage if route is empty
	 */
	private initRedirectToHomePage(): void {
		const isHomepage = this.route.snapshot.children.length === 0;
		if (isHomepage) {
			this.redirectToRootElement();
		}
	}

	/**
	 * Initialize set screenState to the store
	 */
	private initSetScreenStateByRoute(): void {
		this.setScreenStateByRoute();
	}

	/**
	 * set the ScreenState by children avalible in the route
	 */
	private setScreenStateByRoute(): void {
		const isDualOutlet = this.route.snapshot.children.length > 1;
		const isRightScreenMaximized =
			this.screenState === ScreenState.RightScreenMaximized;

		if (isDualOutlet && !isRightScreenMaximized) {
			this.splitScreenFacade.setScreenState(ScreenState.SplitScreen);
		} else if (
			!isDualOutlet &&
			this.screenState !== ScreenState.LeftScreenMaximized
		) {
			this.splitScreenFacade.setScreenState(
				ScreenState.LeftScreenMaximized
			);
		}
	}

	/**
	 * Get RootElement and Redirect to HomePage
	 */
	private redirectToRootElement(): void {
		if (!this.rootElementDcvId) {
			const destroySubject$ = new Subject();
			this.topicFacade.getTopicRoot
				.pipe(takeUntil(destroySubject$))
				.subscribe((rootElement) => {
					if (rootElement) {
						this.rootElementDcvId = rootElement.dcv;
						this.routingFacade.home(rootElement.dcv);
						destroySubject$.next();
						destroySubject$.complete();
					}
				});
		} else {
			this.routingFacade.home(this.rootElementDcvId);
		}
	}

	private initFeatureflags(): void {
		this.featureflagFacade
			.getFeatureflag(TooltipFeatures.tooltip)
			.pipe(take(1))
			.subscribe((featureEnabled) => {
				this.tooltipFeatureFlag = featureEnabled;
			});
	}

	/**
	 * Handle Query Parms that are given by the browser
	 */
	private handleQueryParams(): void {
		this.route.queryParams.pipe(take(1)).subscribe((params) => {
			Object.keys(params).forEach((key) => {
				if (key.toLowerCase() === routingQueryValues.treeKey) {
					if (
						params[key] === routingQueryValues.treeOpen &&
						!this.treePanelVisible
					) {
						this.splitScreenFacade.toggleTreePanelVisibility();
					}
				}
			});
		});
	}
}
