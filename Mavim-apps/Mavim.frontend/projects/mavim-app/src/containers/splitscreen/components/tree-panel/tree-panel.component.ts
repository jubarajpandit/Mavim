import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SplitScreenFacade } from '../../service/splitscreen.facade';
import { Observable, Subject } from 'rxjs';
import { TopicTreeFacade } from './services/topic-tree.facade';
import { RoutingFacade } from '../../../../shared/router/service';
import { filter, map, tap, switchMap, takeUntil } from 'rxjs/operators';
import { TreeOptions } from '../../../../shared/tree/models/tree-options.model';

@Component({
	selector: 'mav-tree-panel',
	templateUrl: './tree-panel.component.html',
	styleUrls: ['./tree-panel.component.scss']
})
export class TreePanelComponent implements OnInit {
	public constructor(
		private readonly splitScreenFacade: SplitScreenFacade,
		public readonly treeFacade: TopicTreeFacade,
		private readonly routingFacade: RoutingFacade
	) {}

	@Input() public treePanelVisibility = false;
	@Output() public topicSelected = new EventEmitter<string>();
	@Output() public closeTree = new EventEmitter();

	public get sidebarVisible(): Observable<boolean> {
		return this.splitScreenFacade.sidebarVisibility;
	}

	public treeOptions = {
		showCreate: true,
		showMove: true
	} as TreeOptions;

	public ngOnInit(): void {
		this.setupTree();
	}

	public onClose(): void {
		this.closeTree.emit();
	}

	public onTopicSelected(dcvId: string): void {
		this.topicSelected.emit(dcvId);
	}

	private setupTree(): void {
		const routerSubject = new Subject();

		this.routingFacade.queue
			.pipe(
				filter((queue: string[]) => queue.length > 0),
				map((queue) => queue[queue.length - 1]),
				tap((currentLocationTopic) => {
					if (!currentLocationTopic) {
						throw new Error(
							'Unable to build the tree. current topicId is unknown.'
						);
					}
					this.treeFacade.ExpandTo(currentLocationTopic);
				}),
				switchMap(() =>
					this.treeFacade.selectNodes.pipe(
						filter((nodes) => nodes && nodes.length > 0),
						map((nodes) => !!nodes),
						takeUntil(routerSubject)
					)
				),
				takeUntil(routerSubject)
			)
			.subscribe(
				() => {
					this.unsubscribe(routerSubject);
				},
				() => {
					this.unsubscribe(routerSubject);
				}
			);
	}

	private unsubscribe(subject: Subject<unknown>): void {
		subject.next();
		subject.complete();
	}
}
