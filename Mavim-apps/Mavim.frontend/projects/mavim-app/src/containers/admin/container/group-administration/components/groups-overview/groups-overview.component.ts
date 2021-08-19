import { Component, Input } from '@angular/core';
import { Group } from '../../model/group';

@Component({
	selector: 'mav-groups-overview',
	templateUrl: './groups-overview.component.html'
})
export class GroupsOverviewComponent {
	@Input() public groups: Group[];
	@Input() public columnDevision = 12;
}
