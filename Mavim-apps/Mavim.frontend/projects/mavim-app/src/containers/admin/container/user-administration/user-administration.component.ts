import { Component, OnDestroy, OnInit } from '@angular/core';
import { RoutingFacade } from '../../../../shared/router/service';
import { AdminUserFacade } from './service/admin-user.facade';
import { Router } from '@angular/router';
import { Authorization } from '../../../../shared/authorization/models/authorization';
import { MsalService } from '@azure/msal-angular';
import { from, Subject } from 'rxjs';
import { filter, take, takeUntil } from 'rxjs/operators';
import { scopes } from '../../../../shared/security/constants';
import { AuthResponse } from 'msal';

@Component({
	selector: 'mav-user-administration',
	templateUrl: './user-administration.component.html',
	styleUrls: ['./user-administration.component.scss']
})
export class UserAdministrationComponent implements OnInit, OnDestroy {
	public constructor(
		private readonly routingFacade: RoutingFacade,
		private readonly router: Router,
		public readonly userFacade: AdminUserFacade,
		public readonly authService: MsalService
	) {}

	public showScreen = false;
	public currentUserEmail = undefined;

	private readonly timeoutMessage = 3000;
	private readonly timeoutMessageSubscription = new Subject();

	public ngOnInit(): void {
		this.setCurrentUserEmail();
		this.clearMessageTimeout();
	}

	public ngOnDestroy(): void {
		this.timeoutMessageSubscription.next();
		this.timeoutMessageSubscription.complete();
	}

	public toggleUserScreen(): void {
		this.router.navigateByUrl(`/admin/users/add`);
	}

	public backToHome(): void {
		this.routingFacade.home(undefined);
	}

	public editUser(user: Authorization): void {
		if (user.email === this.currentUserEmail) return;
		this.router.navigateByUrl(`/admin/users/edit/${user.id}`);
	}

	public deleteUser(user: Authorization): void {
		if (user.email === this.currentUserEmail) return;
		this.router.navigateByUrl(`/admin/users/delete/${user.id}`);
	}

	public onActivate(): void {
		this.showScreen = true;
	}

	public componentRemoved(): void {
		this.showScreen = false;
	}

	public closeScreen(): void {
		this.showScreen = false;
		this.router.navigateByUrl(`/admin/users`);
	}

	public removeMessage(): void {
		this.userFacade.removeMessage();
	}

	private setCurrentUserEmail(): void {
		from(this.authService.acquireTokenSilent({ scopes }))
			.pipe(take(1))
			.subscribe((auth: AuthResponse) => {
				const { email } = auth.idTokenClaims;
				this.currentUserEmail = email?.toLowerCase();
			});
	}

	private clearMessageTimeout(): void {
		this.userFacade.responseMessage
			.pipe(
				filter((m) => !!m),
				takeUntil(this.timeoutMessageSubscription)
			)
			.subscribe(() => {
				const timeout = setTimeout(() => {
					this.userFacade.removeMessage();
					clearTimeout(timeout);
				}, this.timeoutMessage);
			});
	}
}
