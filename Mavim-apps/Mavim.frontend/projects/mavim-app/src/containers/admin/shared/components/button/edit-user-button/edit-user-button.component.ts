import { Component, Input } from '@angular/core';
import { iconSingleNeutralActionsAddPath } from '../../../../../../shared/styles/icons';

@Component({
	selector: 'mav-edit-user-button',
	templateUrl: './edit-user-button.component.html',
	styleUrls: ['./edit-user-button.component.scss']
})
export class EditUserButtonComponent {
	public iconPath = iconSingleNeutralActionsAddPath;
	@Input() public disabled = false;
}
