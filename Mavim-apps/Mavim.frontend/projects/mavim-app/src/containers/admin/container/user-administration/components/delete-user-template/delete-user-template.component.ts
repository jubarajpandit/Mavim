import { Component, Input } from '@angular/core';
import { ModalTemplateComponent } from '../../../../../../shared/modal/components/modaltemplate/modaltemplate';
import { User } from '../../model/authorization';

@Component({
	selector: 'mav-delete-user-template',
	templateUrl: './delete-user-template.component.html',
	styleUrls: ['./delete-user-template.component.scss']
})
export class DeleteUserTemplateComponent extends ModalTemplateComponent {
	@Input() public user: User;

	public accept(): void {
		this.modelClose.emit(true);
	}

	public cancel(): void {
		this.modelClose.emit(false);
	}
}
