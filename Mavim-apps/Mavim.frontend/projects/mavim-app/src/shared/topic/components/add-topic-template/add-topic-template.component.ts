import { Component, Input, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { TopicMetaFacade } from '../../../topic-meta/services/topic-meta.facade';
import { FlatTreeNode } from '../../../tree/models/flat-tree-node.model';
import { TopicFacade } from '../../services/topic.facade';
import { AddTopicBaseTemplateComponent } from './add-topic-base-template.component';

@Component({
	templateUrl: './add-topic-base-template.component.html',
	styleUrls: ['./add-topic-base-template.component.scss']
})
export class AddTopicTemplateComponent
	extends AddTopicBaseTemplateComponent
	implements OnInit
{
	public constructor(
		private readonly topicFacade: TopicFacade,
		public readonly topicMetaFacade: TopicMetaFacade
	) {
		super(topicMetaFacade);
	}

	@Input() public topic: FlatTreeNode;
	private parentId: string;

	public ngOnInit(): void {
		this.topicFacade
			.getTopicByDcv(this.topic.dcvId)
			.pipe(take(1))
			.subscribe((topic) => (this.parentId = topic?.parent));
		this.topicMetaFacade.loadTypes(this.parentId);
		this.subscribeToTypesValueChanges();
	}

	public accept(): void {
		this.topicFacade.createTopic(
			this.topic.dcvId,
			this.titleFormControl.value,
			this.typeFormControl.value,
			this.iconFormControl.value
		);
		this.modelClose.emit(true);
	}
}
