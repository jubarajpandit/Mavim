import { Component, Input } from '@angular/core';
import { iconSingleNeutralActionsAddPath } from '../../../../../../shared/styles/icons';

@Component({
	selector: 'mav-delete-user-button',
	templateUrl: './delete-user-button.component.html',
	styleUrls: ['./delete-user-button.component.scss']
})
export class DeleteUserButtonComponent {
	public iconPath = iconSingleNeutralActionsAddPath;
	@Input() public disabled = false;
}
