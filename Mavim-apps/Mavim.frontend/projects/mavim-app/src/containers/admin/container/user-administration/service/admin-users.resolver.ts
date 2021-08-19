import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { AdminUserFacade } from './admin-user.facade';

@Injectable({ providedIn: 'root' })
export class UsersResolver implements Resolve<boolean> {
	public constructor(private readonly userFacade: AdminUserFacade) {}

	public resolve(): boolean {
		this.userFacade.loadAuthorizedUsers();
		return true;
	}
}
