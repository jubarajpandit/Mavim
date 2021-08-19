import { Component, Input } from '@angular/core';
import { AdminUserFacade } from '../../service/admin-user.facade';
import { User } from '../../model/authorization';
import { DeleteUserTemplateComponent } from '../delete-user-template/delete-user-template.component';
import { take } from 'rxjs/operators';
import { ModalFactoryService } from '../../../../../../shared/modal/components/modalfactory/modalfactory.service';

@Component({
	selector: 'mav-delete-user',
	templateUrl: './delete-user.component.html',
	styleUrls: ['./delete-user.component.scss']
})
export class DeleteUserComponent {
	public constructor(
		public readonly userFacade: AdminUserFacade,
		private readonly modalFactoryService: ModalFactoryService
	) {}

	@Input() public user: User;

	public showDeleteUserModal(): void {
		this.modalFactoryService
			.create(DeleteUserTemplateComponent, [], { user: this.user })
			.pipe(take(1))
			.subscribe((event) => {
				if (event) {
					this.userFacade.deleteAuthorizedUser(this.user);
				}
			});
	}
}
