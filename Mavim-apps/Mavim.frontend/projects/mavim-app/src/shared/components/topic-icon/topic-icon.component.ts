import { Component, Input } from '@angular/core';

@Component({
	selector: 'mav-topicicon',
	templateUrl: './topic-icon.component.html',
	styleUrls: ['./topic-icon.component.scss']
})
export class TopicIconComponent {
	@Input() public name: string;
}
