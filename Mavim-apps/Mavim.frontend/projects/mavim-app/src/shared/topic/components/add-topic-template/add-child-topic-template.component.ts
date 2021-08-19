import { Component, Input, OnInit } from '@angular/core';
import { filter, map, takeUntil } from 'rxjs/operators';
import { TopicTreeFacade } from '../../../../containers/splitscreen/components/tree-panel/services/topic-tree.facade';
import { TopicMetaFacade } from '../../../topic-meta/services/topic-meta.facade';
import { FlatTreeNode } from '../../../tree/models/flat-tree-node.model';
import { Topic } from '../../models/topic.model';
import { TopicFacade } from '../../services/topic.facade';
import { AddTopicBaseTemplateComponent } from './add-topic-base-template.component';

@Component({
	selector: 'mav-add-child-topic-template',
	templateUrl: './add-topic-base-template.component.html',
	styleUrls: ['./add-topic-base-template.component.scss']
})
export class AddChildTopicTemplateComponent
	extends AddTopicBaseTemplateComponent
	implements OnInit
{
	public constructor(
		private readonly topicFacade: TopicFacade,
		private readonly topicTreeFacade: TopicTreeFacade,
		public readonly topicMetaFacade: TopicMetaFacade
	) {
		super(topicMetaFacade);
	}

	@Input() public parentTopic: FlatTreeNode;

	public ngOnInit(): void {
		this.topicMetaFacade.loadTypes(this.parentTopic.dcvId);
		this.subscribeToTypesValueChanges();
	}

	public accept(): void {
		if (this.parentTopic.isExpandable && !this.parentTopic.isExpanded) {
			this.topicFacade.loadTopicChildren(this.parentTopic.dcvId);

			const childLevel = this.parentTopic.level + 1;

			this.topicFacade
				.getChildTopicsByDcv(this.parentTopic.dcvId)
				.pipe(
					filter((children) => children && children.length > 0),
					map((children) =>
						children.map((topic) => this.map(topic, childLevel))
					),
					takeUntil(this.destroySubscription)
				)
				.subscribe((children) => {
					this.closeModal(true);
					this.topicTreeFacade.Add(this.parentTopic, children);
					this.topicFacade.createChildtopic(
						this.parentTopic.dcvId,
						this.titleFormControl.value,
						this.typeFormControl.value,
						this.iconFormControl.value
					);
				});
		} else {
			this.closeModal(true);
			this.topicFacade.createChildtopic(
				this.parentTopic.dcvId,
				this.titleFormControl.value,
				this.typeFormControl.value,
				this.iconFormControl.value
			);
		}
	}

	private map(topic: Topic, level: number): FlatTreeNode {
		return new FlatTreeNode(
			topic.name,
			topic.dcv,
			topic.icon,
			level,
			topic.orderNumber,
			topic.customIconId,
			topic.hasChildren,
			topic.business?.canDelete,
			topic.isInRecycleBin,
			topic.business?.canCreateChildTopic,
			topic.business?.canCreateTopicAfter
		);
	}
}
