import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
	selector: 'mav-add-group',
	templateUrl: './create-group.component.html'
})
export class CreateGroupComponent {
	public constructor(private readonly router: Router) {}

	public close(): void {
		this.router.navigateByUrl(`/admin/groups`);
	}
}
