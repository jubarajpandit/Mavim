import { Component, Output, EventEmitter, Input, OnInit } from '@angular/core';
import { Subject, Observable, of } from 'rxjs';
import { map, take, takeUntil } from 'rxjs/operators';
import { GenericTreeFacade } from '../../shared/tree/services/generictree.facade';
import { TopicFacade } from '../../shared/topic/services/topic.facade';
import { ErrorService } from '../../shared/notification/services/error.service';
import { FlatTreeNode } from '../../shared/tree/models/flat-tree-node.model';
import { Topic } from '../../shared/topic/models/topic.model';
import { TreeOptions } from '../../shared/tree/models/tree-options.model';
import { ModalFactoryService } from '../../shared/modal/components/modalfactory/modalfactory.service';
import { AddChildTopicTemplateComponent } from '../../shared/topic/components/add-topic-template/add-child-topic-template.component';
import { AddTopicTemplateComponent } from '../../shared/topic/components/add-topic-template/add-topic-template.component';
import { TopicTreeFacade } from '../splitscreen/components/tree-panel/services/topic-tree.facade';
import { DeleteTopicTemplateComponent } from '../../shared/topic/components/delete-topic-template/delete-topic-template.component';
import { FetchStatus } from '../../shared/enums/FetchState';

@Component({
	selector: 'mav-tree-container',
	templateUrl: './tree-container.component.html'
})
export class TreeContainerComponent implements OnInit {
	public constructor(
		private readonly topicFacade: TopicFacade,
		private readonly errorService: ErrorService,
		private readonly modalFactoryService: ModalFactoryService
	) {}

	@Input() public readonly treeFacade: GenericTreeFacade;
	@Input() public readonly options = {
		showCreate: false,
		showMove: false
	} as TreeOptions;

	@Output() public topicSelected = new EventEmitter<string>();

	public readonly successStatusCode = 200;
	public readonly notFoundMainText = 'Tree could not be loaded';
	public readonly notFoundSubText =
		'If this problem persists, please contact your administrator.';

	public ngOnInit(): void {
		if (this.options.showCreate || this.options.showMove) {
			this.subscribeToAllTopicsLoaded();
		}
	}

	public get isTreeFetched(): Observable<boolean> {
		return this.treeFacade
			? this.treeFacade.selectFetchStatus.pipe(
					map((fetchStatus) => fetchStatus === FetchStatus.Fetched)
			  )
			: of(false);
	}

	public get isTreeLoading(): Observable<boolean> {
		return this.treeFacade
			? this.treeFacade.selectFetchStatus.pipe(
					map((fetchStatus) => fetchStatus === FetchStatus.Loading)
			  )
			: of(false);
	}

	public get isTreeFailed(): Observable<boolean> {
		return this.treeFacade
			? this.treeFacade.selectFetchStatus.pipe(
					map((fetchStatus) => fetchStatus === FetchStatus.Failed)
			  )
			: of(false);
	}

	public onTopicSelected(dcvId: string): void {
		this.treeFacade.select(dcvId);
		this.topicSelected.emit(dcvId);
	}

	public handleNodeCollapsed(parent: FlatTreeNode): void {
		this.treeFacade.Remove(parent);
	}

	public handleMoveToTop(node: FlatTreeNode): void {
		if (node === undefined) return;
		this.treeFacade.moveToTop(node.dcvId);
	}

	public handleMoveToBottom(node: FlatTreeNode): void {
		if (node === undefined) return;
		this.treeFacade.moveToBottom(node.dcvId);
	}

	public handleMoveUp(node: FlatTreeNode): void {
		if (node === undefined) return;
		this.treeFacade.moveUp(node.dcvId);
	}

	public handleMoveDown(node: FlatTreeNode): void {
		if (node === undefined) return;
		this.treeFacade.moveDown(node.dcvId);
	}

	public handleMoveLevelUp(node: FlatTreeNode): void {
		if (node === undefined) return;
		this.treeFacade.moveLevelUp(node.dcvId);
	}

	public handleMoveLevelDown(node: FlatTreeNode): void {
		if (node === undefined) return;

		this.treeFacade.selectNodes.pipe(take(1)).subscribe((nodes) => {
			let newParent: FlatTreeNode;
			for (
				let i = nodes.findIndex((n) => n.dcvId === node.dcvId) - 1;
				i >= 0;
				i--
			) {
				if (nodes[i].level < node.level) {
					break;
				}
				if (nodes[i].level === node.level) {
					newParent = nodes[i];
					break;
				}
			}

			if (newParent && newParent.isExpandable && !newParent.isExpanded) {
				let fetchChildNodes = false;
				let subCompleted = false;
				const childTopicsLoaded = new Subject();
				this.topicFacade
					.getChildTopicsByDcv(newParent.dcvId)
					.pipe(takeUntil(childTopicsLoaded))
					.subscribe(
						(children) => {
							if (
								!(children && children.length > 0) &&
								!fetchChildNodes
							) {
								fetchChildNodes = true;
								this.loadTopicChildren(newParent);
							} else if (
								children &&
								children.length > 0 &&
								!subCompleted
							) {
								subCompleted = true;
								this.addTopicChildrenToTreeData(
									newParent,
									children,
									fetchChildNodes
								);
								this.treeFacade.moveLevelDown(node.dcvId);
								childTopicsLoaded.next();
								childTopicsLoaded.complete();
							}
						},
						(error) =>
							this.errorService.handleServiceError(
								error,
								'childTopics'
							)
					);
			} else {
				this.treeFacade.moveLevelDown(node.dcvId);
			}
		});
	}

	public handleNodeExpanded(parent: FlatTreeNode): void {
		let fetchChildNodes = false;
		const childTopicsLoaded = new Subject();
		this.topicFacade
			.getChildTopicsByDcv(parent.dcvId)
			.pipe(takeUntil(childTopicsLoaded))
			.subscribe(
				(children) => {
					if (
						!(children && children.length > 0) &&
						!fetchChildNodes
					) {
						fetchChildNodes = true;
						this.loadTopicChildren(parent);
					} else if (children && children.length > 0) {
						this.addTopicChildrenToTreeData(
							parent,
							children,
							fetchChildNodes
						);
						childTopicsLoaded.next();
						childTopicsLoaded.complete();
					}
				},
				(error) =>
					this.errorService.handleServiceError(error, 'childTopics')
			);
	}

	public openAddTopicModal(event: FlatTreeNode): void {
		this.modalFactoryService
			.create(
				AddTopicTemplateComponent,
				[
					{
						provide: TopicFacade,
						useValue: this.topicFacade
					}
				],
				{ modalTitle: 'Add Topic', topic: event }
			)
			.pipe(take(1))
			.subscribe();
	}

	public openAddChildTopicModal(event: FlatTreeNode): void {
		this.modalFactoryService
			.create(
				AddChildTopicTemplateComponent,
				[
					{
						provide: TopicFacade,
						useValue: this.topicFacade
					},
					{
						provide: TopicTreeFacade,
						useValue: this.treeFacade
					}
				],
				{ modalTitle: 'Add Child Topic', parentTopic: event }
			)
			.pipe(take(1))
			.subscribe();
	}

	public openDeleteTopicModal(event: FlatTreeNode): void {
		this.modalFactoryService
			.create(
				DeleteTopicTemplateComponent,
				[
					{
						provide: TopicFacade,
						useValue: this.topicFacade
					},
					{
						provide: TopicTreeFacade,
						useValue: this.treeFacade
					}
				],
				{ modalTitle: 'Remove Topic', topic: event }
			)
			.pipe(take(1))
			.subscribe();
	}

	private loadTopicChildren(parent: FlatTreeNode): void {
		this.topicFacade.loadTopicChildren(parent.dcvId);
		this.treeFacade.ToggleLoading(parent);
	}

	private addTopicChildrenToTreeData(
		parent: FlatTreeNode,
		children: Topic[],
		fetchChildNodes: boolean
	): void {
		if (fetchChildNodes) {
			this.treeFacade.ToggleLoading(parent);
		}
		this.treeFacade.Add(
			parent,
			children.map(
				(topic) =>
					new FlatTreeNode(
						topic.name,
						topic.dcv,
						topic.icon,
						parent.level + 1,
						topic.orderNumber,
						topic.customIconId,
						topic.hasChildren,
						topic.business?.canDelete,
						topic.isInRecycleBin,
						topic.business?.canCreateChildTopic,
						topic.business?.canCreateTopicAfter
					)
			)
		);
	}

	private subscribeToAllTopicsLoaded(): void {
		const options = { ...this.options };
		this.topicFacade.getAllTopicsLoaded.subscribe((isLoaded) => {
			this.options.showCreate = isLoaded ? options.showCreate : false;
			this.options.showMove = isLoaded ? options.showMove : false;
		});
	}
}
