import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, Output, EventEmitter, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { Role } from '../../../authorization/enums/role';
import { AuthorizationFacade } from '../../../authorization/service/authorization.facade';
import { ListItem } from '../../../components/menu-list/models/list-item.model';
import { FeatureflagFacade } from '../../../featureflag/service/featureflag.facade';
import { TreeActions } from '../../enums/tree-actions.enum';
import { TreeFeatures } from '../../enums/tree-feature-flags.enum';
import { FlatTreeNode } from '../../models/flat-tree-node.model';
import { TreeOptions } from '../../models/tree-options.model';

@Component({
	selector: 'mav-tree-cdk',
	templateUrl: 'tree-cdk.component.html',
	styleUrls: ['tree-cdk.component.scss']
})
export class TreeCdkComponent implements OnInit {
	public constructor(
		private readonly authFacade: AuthorizationFacade,
		private readonly featureFlagFacade: FeatureflagFacade
	) {
		this.treeControl = new FlatTreeControl<FlatTreeNode>(
			this.getLevel,
			this.isExpandable
		);
	}

	@Input() public nodes: FlatTreeNode[];
	@Input() public options: TreeOptions;

	@Output() public topicSelected = new EventEmitter<string>();
	@Output() public nodeExpanded = new EventEmitter<FlatTreeNode>();
	@Output() public nodeCollapsed = new EventEmitter<FlatTreeNode>();

	@Output() public addTopic = new EventEmitter<FlatTreeNode>();
	@Output() public addChildTopic = new EventEmitter<FlatTreeNode>();
	@Output() public deleteTopic = new EventEmitter<FlatTreeNode>();
	@Output() public moveToTop = new EventEmitter<FlatTreeNode>();
	@Output() public moveUp = new EventEmitter<FlatTreeNode>();
	@Output() public moveDown = new EventEmitter<FlatTreeNode>();
	@Output() public moveToBottom = new EventEmitter<FlatTreeNode>();
	@Output() public moveLevelUp = new EventEmitter<FlatTreeNode>();
	@Output() public moveLevelDown = new EventEmitter<FlatTreeNode>();

	public get isAuthorized(): Observable<boolean> {
		return this.authFacade.getAuthorization.pipe(
			map((auth) => auth?.role !== Role.Subscriber)
		);
	}
	public showCreateMenu = false;
	public showMoveMenu = false;
	public menuTopic: FlatTreeNode = undefined;
	public menuPosition: DOMRect = undefined;
	public menuTopicElement: HTMLElement = undefined;
	public readonly moveMenuHeight = 230;
	public readonly createMenuHeight = 98;
	public readonly menuOffsetY = -84;
	public readonly menuOffsetX = 75;
	public readonly windowHeight = window.innerHeight + this.menuOffsetY;
	public get createMenuList(): ListItem[] {
		return this.getCreateMenu();
	}

	public readonly moveMenuList: ListItem[] = [
		{
			name: TreeActions.MoveToTop,
			isHeader: false,
			icon: 'move-topic-top-icon'
		},
		{
			name: TreeActions.MoveUp,
			isHeader: false,
			icon: 'move-topic-up-icon'
		},
		{
			name: TreeActions.MoveDown,
			isHeader: false,
			icon: 'move-topic-down-icon'
		},
		{
			name: TreeActions.MoveToBottom,
			isHeader: false,
			icon: 'move-topic-bottom-icon'
		},
		{
			name: TreeActions.MoveLevelUp,
			isHeader: false,
			icon: 'move-topic-level-up-top-icon'
		},
		{
			name: TreeActions.MoveLevelDown,
			isHeader: false,
			icon: 'move-topic-level-down-icon'
		}
	];

	public features = TreeFeatures;

	public treeControl: FlatTreeControl<FlatTreeNode>;

	private readonly treePadding = 1.375;
	private deleteFeature = false;

	public ngOnInit(): void {
		this.featureFlagFacade
			.getFeatureflag(TreeFeatures.deleteTopic)
			.pipe(take(1))
			.subscribe((featureEnabled) => {
				this.deleteFeature = featureEnabled;
			});
	}

	/** Handle expand/collapse behaviors */
	public toggleNode(node: FlatTreeNode): void {
		this.hideMenu();

		if (!node.isExpanded) {
			this.nodeExpanded.emit(node);
		} else {
			this.nodeCollapsed.emit(node);
		}
	}

	public handleToggleCreateMenu(event: Event, node: FlatTreeNode): void {
		event.stopPropagation();
		this.showCreateMenu =
			this.menuTopic === node ? !this.showCreateMenu : true;
		this.menuPosition = (
			event.target as HTMLDivElement
		).getClientRects()[0];
		this.menuTopic = node;
		this.showMoveMenu = false;
		this.menuTopicElement?.classList?.remove('hover');
		if (this.showCreateMenu) {
			const menuTopicElement = (event.target as HTMLDivElement)
				.parentElement.parentElement;
			menuTopicElement.classList.add('hover');
			this.menuTopicElement = menuTopicElement;
		}
		this.menuPosition = (
			event.target as HTMLDivElement
		).getClientRects()[0];
	}

	public handleToggleMoveMenu(event: Event, node: FlatTreeNode): void {
		event.stopPropagation();
		this.showMoveMenu = this.menuTopic === node ? !this.showMoveMenu : true;
		this.menuPosition = (
			event.target as HTMLDivElement
		).getClientRects()[0];
		this.menuTopic = node;
		this.showCreateMenu = false;
		this.menuTopicElement?.classList?.remove('hover');
		if (this.showMoveMenu) {
			const menuTopicElement = (event.target as HTMLDivElement)
				.parentElement.parentElement;
			menuTopicElement.classList.add('hover');
			this.menuTopicElement = menuTopicElement;
		}
		this.menuPosition = (
			event.target as HTMLDivElement
		).getClientRects()[0];
	}

	public handleMenuItemClicked(item: string): void {
		this.hideMenu();

		switch (item) {
			case TreeActions.AddTopic:
				this.addTopic.emit(this.menuTopic);
				break;
			case TreeActions.AddChildTopic:
				this.addChildTopic.emit(this.menuTopic);
				break;
			case TreeActions.DeleteTopic:
				this.deleteTopic.emit(this.menuTopic);
				break;
			case TreeActions.MoveToTop:
				this.moveToTop.emit(this.menuTopic);
				break;
			case TreeActions.MoveUp:
				this.moveUp.emit(this.menuTopic);
				break;
			case TreeActions.MoveDown:
				this.moveDown.emit(this.menuTopic);
				break;
			case TreeActions.MoveToBottom:
				this.moveToBottom.emit(this.menuTopic);
				break;
			case TreeActions.MoveLevelUp:
				this.moveLevelUp.emit(this.menuTopic);
				break;
			case TreeActions.MoveLevelDown:
				this.moveLevelDown.emit(this.menuTopic);
				break;
			default:
				throw Error('Unknow tree action');
		}
	}

	public getLevel = (node: FlatTreeNode): number => node.level;
	public isExpandable = (node: FlatTreeNode): boolean => node.isExpandable;
	public hasChild = (_: number, node: FlatTreeNode): boolean =>
		node.isExpandable;
	public getPadding = (node: FlatTreeNode): string =>
		`${node.level * this.treePadding}rem`;

	public selectNode(node: FlatTreeNode): void {
		this.hideMenu();
		this.topicSelected.emit(node.dcvId);
	}

	public isSelectedNode(selectedNode: FlatTreeNode): boolean {
		return (
			this.nodes?.find((node) => node.dcvId === selectedNode.dcvId)
				?.isSelected || false
		);
	}

	public get isAnyNodeLoading(): boolean {
		return this.nodes?.some((node) => node.isLoading);
	}

	public getMenuYPosition(menuHeight: number): number {
		return this.menuPosition?.top + this.menuOffsetY + menuHeight >
			this.windowHeight
			? this.menuPosition?.top + this.menuOffsetY - menuHeight
			: this.menuPosition?.top + this.menuOffsetY;
	}

	public hideMenu(): void {
		this.menuTopicElement?.classList?.remove('hover');
		this.showCreateMenu = false;
		this.showMoveMenu = false;
	}

	public canCreateTopic(node: FlatTreeNode): boolean {
		return (
			node.canCreateTopicAfter ||
			node.canCreateChildTopic ||
			node.canDelete
		);
	}

	private getCreateMenu(): ListItem[] {
		const menu: ListItem[] = [];

		if (this.menuTopic?.canCreateTopicAfter)
			menu.push({
				name: TreeActions.AddTopic,
				isHeader: false,
				icon: 'add-topic-icon'
			});

		if (this.menuTopic?.canCreateChildTopic)
			menu.push({
				name: TreeActions.AddChildTopic,
				isHeader: false,
				icon: 'add-child-topic-icon'
			});

		if (this.deleteFeature && this.menuTopic?.canDelete) {
			menu.push({
				name: TreeActions.DeleteTopic,
				isHeader: false,
				icon: 'delete-topic-icon',
				class: 'delete-option'
			});
		}
		return menu;
	}
}
