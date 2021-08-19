import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Topic } from '../../topic/models/topic.model';

@Component({
	selector: 'mav-topic-children',
	templateUrl: './topic-children.component.html',
	styleUrls: ['./topic-children.component.scss']
})
export class TopicChildrenComponent {
	@Input() public childTopics: Topic[];
	@Output() public internalDcvId: EventEmitter<string> =
		new EventEmitter<string>();

	public emitInternalLink(dcv: string): void {
		this.internalDcvId.emit(dcv);
	}
}
