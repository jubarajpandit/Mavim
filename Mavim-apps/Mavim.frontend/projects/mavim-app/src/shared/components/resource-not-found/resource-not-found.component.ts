import { Component, Input } from '@angular/core';

@Component({
	selector: 'mav-resource-not-found',
	templateUrl: './resource-not-found.component.html',
	styleUrls: ['./resource-not-found.component.scss']
})
export class ResourceNotFoundComponent {
	@Input() public mainText = 'Resource not found';
	@Input() public subText = 'The link to this resource is not valid.';
}
