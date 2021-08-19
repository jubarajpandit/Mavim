import { Component, Input } from '@angular/core';
import { iconSingleNeutralActionsAddPath } from '../../../../../../shared/styles/icons';

@Component({
	selector: 'mav-add-user-button',
	templateUrl: './add-user-button.component.html',
	styleUrls: ['./add-user-button.component.scss']
})
export class AddUserButtonComponent {
	public iconPath = iconSingleNeutralActionsAddPath;
	@Input() public disabled = false;
}
