import { Component, Input } from '@angular/core';
import { take } from 'rxjs/operators';
import { ModalTemplateComponent } from '../../../modal/components/modaltemplate/modaltemplate';
import { FlatTreeNode } from '../../../tree/models/flat-tree-node.model';
import { TopicFacade } from '../../services/topic.facade';

@Component({
	templateUrl: './delete-topic-template.component.html',
	styleUrls: ['./delete-topic-template.component.scss']
})
export class DeleteTopicTemplateComponent extends ModalTemplateComponent {
	public constructor(private readonly topicFacade: TopicFacade) {
		super();
	}

	@Input() public topic: FlatTreeNode;
	@Input() public modalTitle: string;

	public accept(): void {
		this.topicFacade.getRecycleBin.pipe(take(1)).subscribe((recyleBin) => {
			this.topicFacade.deleteTopic(this.topic.dcvId, recyleBin?.dcv);
		});

		this.closeModal(true);
	}
}
