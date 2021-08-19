import { Component, OnDestroy } from '@angular/core';
import { RoutingFacade } from '../../../../shared/router/service';
import { SplitScreenFacade } from '../../service/splitscreen.facade';
import { Subject, combineLatest, BehaviorSubject, Observable } from 'rxjs';
import { takeUntil, tap } from 'rxjs/operators';
import { ErrorService } from '../../../../shared/notification/services/error.service';
import { TopicFacade } from '../../../../shared/topic/services/topic.facade';
import { ScreenState } from '../../enums/screenState';

@Component({
	selector: 'mav-sidebar-button',
	templateUrl: './sidebar-button.component.html',
	styleUrls: ['./sidebar-button.component.scss']
})
export class SidebarButtonComponent implements OnDestroy {
	public constructor(
		private readonly routingFacade: RoutingFacade,
		private readonly topicFacade: TopicFacade,
		private readonly splitscreenFacade: SplitScreenFacade,
		private readonly errorService: ErrorService
	) {
		this.subscribeToSidebarContent();
	}

	public isLoading = false;
	public title: string = undefined;
	public titleEmitter$ = new BehaviorSubject<string>(this.title);
	private readonly destroySubject$ = new Subject();

	public ngOnDestroy(): void {
		this.destroySubject$.next();
		this.destroySubject$.complete();
	}

	public handleBack(): void {
		this.routingFacade.back();
	}

	public getSidebarVisibility(): Observable<boolean> {
		return this.splitscreenFacade.sidebarVisibility;
	}

	private get minimalOpenScreens(): boolean {
		const minimalOpenScreensValue = 2;
		return history && history.length > minimalOpenScreensValue;
	}

	private get minimumOpenScreensWithTreenOpen(): boolean {
		const minimumOpenScreensWithTreenOpenValue = 1;
		return history && history.length > minimumOpenScreensWithTreenOpenValue;
	}

	private subscribeToSidebarContent(): void {
		combineLatest([
			this.routingFacade.queue,
			this.splitscreenFacade.treePanelVisibility,
			this.splitscreenFacade.sidebarVisibility,
			this.splitscreenFacade.screenState
		])
			.pipe(takeUntil(this.destroySubject$))
			.subscribe(
				([history, treePanelVisible, sidebarVisible, screenState]) => {
					const dcvid = this.GetPreviousDcvId(
						treePanelVisible,
						screenState,
						history
					);

					if (dcvid) {
						this.getTopicTitle(dcvid);
					}

					// Only allowed for aliens, next level programming
					if (sidebarVisible !== !!dcvid) {
						this.splitscreenFacade.toggleSidebarVisibility();
					}
				}
			);
	}

	private GetPreviousDcvId(
		treePanelVisible: boolean,
		screenState: ScreenState,
		history: string[]
	): string {
		if (
			!treePanelVisible &&
			this.minimalOpenScreens &&
			screenState !== ScreenState.RightScreenMaximized
		) {
			const thirdToLastOffset = 3;
			return history[history.length - thirdToLastOffset];
		}
		if (
			(treePanelVisible ||
				screenState === ScreenState.RightScreenMaximized) &&
			this.minimumOpenScreensWithTreenOpen
		) {
			const secondToLastOffset = 2;
			return history[history.length - secondToLastOffset];
		}

		return undefined;
	}

	private getTopicTitle(dcvid: string): void {
		let fetchTopic = false;
		const destroySubscription$ = new Subject();
		this.topicFacade
			.getTopicByDcv(dcvid)
			.pipe(
				tap((topic) => {
					if (!topic && !fetchTopic) {
						this.title = '';
						this.titleEmitter$.next(this.title);
						this.isLoading = true;
						fetchTopic = true;
						this.topicFacade.loadTopicByDcv(dcvid);
					}
				}),
				takeUntil(destroySubscription$),
				takeUntil(this.destroySubject$)
			)
			.subscribe((topic) => {
				if (topic) {
					this.title = topic.name;
					this.titleEmitter$.next(this.title);
					this.isLoading = false;
					destroySubscription$.next();
					destroySubscription$.complete();
				}
			});
	}
}
