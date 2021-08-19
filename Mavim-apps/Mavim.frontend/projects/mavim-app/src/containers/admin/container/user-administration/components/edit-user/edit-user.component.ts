import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Role } from '../../../../../../shared/authorization/enums/role';
import { AdminUserFacade } from '../../service/admin-user.facade';
import { User } from '../../model/authorization';
import { FormGroup, FormControl } from '@angular/forms';
import { map, mergeMap, filter, takeUntil } from 'rxjs/operators';
import { PatchUserRole } from '../../interface/patchUserRole';
import { Subject } from 'rxjs';

@Component({
	selector: 'mav-edit-user',
	templateUrl: './edit-user.component.html',
	styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit, OnDestroy {
	public get isRoleChanged(): boolean {
		return this.role?.value !== this.user?.role;
	}

	private get role(): FormControl {
		return this.usersRole.get('role') as FormControl;
	}
	public constructor(
		private readonly route: ActivatedRoute,
		private readonly router: Router,
		public readonly userFacade: AdminUserFacade
	) {}
	public roles = Object.keys(Role);

	public usersRole = new FormGroup({
		email: new FormControl({ value: '', disabled: true }),
		role: new FormControl({ value: undefined })
	});

	public user: User = undefined;

	private readonly subscription = new Subject();

	public ngOnInit(): void {
		const USERID = 'userid';
		this.route.params
			.pipe(
				map((params) => params[USERID] as string),
				mergeMap((userId: string) =>
					this.userFacade
						.authorizedUserById(userId)
						.pipe(filter((user) => !!user))
				),
				takeUntil(this.subscription)
			)
			.subscribe((user) => {
				this.user = user;
				const { email, role } = this.user;
				this.usersRole.patchValue({ email, role });
			});
	}

	public ngOnDestroy(): void {
		this.subscription.next();
		this.subscription.complete();
	}

	public editUser(): void {
		const role = this.role.value as Role;
		const { id } = this.user;
		this.userFacade.editAuthorizedUser({ id, role } as PatchUserRole);
		this.close();
	}

	public close(): void {
		this.router.navigateByUrl(`/admin/users`);
	}
}
