import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { TabboxItem } from '../../interface/tabboxitem';
import { MsalService } from '@azure/msal-angular';
import { RegexUtils } from '../../../../../../shared/utils';
import { AddUserRole } from '../../interface/addUserRole';
import { FormControl, FormGroup } from '@angular/forms';
import { Role } from '../../../../../../shared/authorization/enums/role';
import { scopes } from '../../../../../../shared/security/constants';
import { AuthResponse } from 'msal';
import { from } from 'rxjs';
import { take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AdminUserFacade } from '../../service/admin-user.facade';
import {
	iconCheckCirclePath,
	iconRemoveCirclePath
} from '../../../../../../shared/styles/icons';

@Component({
	selector: 'mav-add-user',
	templateUrl: './add-user.component.html',
	styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {
	public constructor(
		private readonly authService: MsalService,
		private readonly router: Router,
		public readonly userFacade: AdminUserFacade
	) {}

	public validEmailIconPath = iconCheckCirclePath;
	public invalidEmailIconPath = iconRemoveCirclePath;
	public emailMessage = false;
	public globalMessage = undefined;
	public users: TabboxItem[] = [];
	public isFetchData = false;
	public roles = Object.keys(Role);
	public get addUserButtonText(): string {
		return this.users.length > 1 ? 'Add users' : 'Add user';
	}
	public usersRole = new FormGroup({
		email: new FormControl(''),
		role: new FormControl(Role.Subscriber)
	});

	@ViewChild('name') public nameField: ElementRef<HTMLElement>;

	private validDomain: string;
	private get email(): FormControl {
		return this.usersRole.get('email') as FormControl;
	}
	private get role(): FormControl {
		return this.usersRole.get('role') as FormControl;
	}

	public get validUsers(): boolean {
		const validAmountOfUsers = this.users.length > 0;
		const invalidUser = !!this.users.find((u) => !u.valid);
		return validAmountOfUsers && !invalidUser;
	}

	public ngOnInit(): void {
		this.setDomainFromJwt();
	}

	public addUserToList(): void {
		if (this.emailMessage) {
			this.emailMessage = false;
		}
		const value = this.email.value as string;
		const valid = this.validation(value);
		this.users = [
			...this.users,
			{
				name: value,
				valid
			}
		];
		this.emailMessage = !valid;
		this.email.reset('');
	}

	public focusInput(): void {
		this.nameField.nativeElement.focus();
	}

	public removeUser(): void {
		const value = this.email.value as string;
		if (value.length === 0 && this.users.length > 0) {
			this.users.pop();
		}
	}

	public close(): void {
		this.router.navigateByUrl(`/admin/users`);
	}

	public addUsersWithRole(): void {
		this.isFetchData = true;
		if (this.users.length === 0) {
			this.emailMessage = true;
			this.isFetchData = false;
			return;
		}
		const userWithRole = this.users
			.filter((u) => u.valid)
			.map((i) => this.map(i));
		this.userFacade.addAuthorizedUsers(userWithRole);
		this.close();
	}

	public closeGlobalMessage(): void {
		this.globalMessage = undefined;
	}
	public closeMessage(): void {
		this.emailMessage = false;
	}

	private map(i: TabboxItem): AddUserRole {
		return { email: i.name, role: this.role.value as Role } as AddUserRole;
	}

	private validation(item: string): boolean {
		return (
			this.isValidEmail(item) &&
			item?.toLowerCase().endsWith(this.validDomain)
		);
	}

	private isValidEmail(email: string): boolean {
		return RegExp(RegexUtils.email()).test(email);
	}

	private setDomainFromJwt(): void {
		from(this.authService.acquireTokenSilent({ scopes }))
			.pipe(take(1))
			.subscribe((auth: AuthResponse) => {
				const { email } = auth.idTokenClaims;
				if (!email && !this.isValidEmail(email)) {
					this.globalMessage =
						'No email scope found, refresh the browser and try again';
					return;
				}
				const itemsToSplit = 2;
				const [, domain] = email.split('@', itemsToSplit);
				this.validDomain = domain?.toLowerCase();
			});
	}
}
