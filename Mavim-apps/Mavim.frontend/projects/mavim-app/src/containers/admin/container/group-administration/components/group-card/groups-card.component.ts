import { Component, Input } from '@angular/core';
import { Role } from '../../model/Role';

@Component({
	selector: 'mav-create-group',
	templateUrl: './groups-card.component.html',
	styleUrls: ['./groups-card.component.scss']
})
export class GroupCardComponent {
	@Input() public title = '';
	@Input() public description = '';
	@Input() public roles: Role[] = [];
}
