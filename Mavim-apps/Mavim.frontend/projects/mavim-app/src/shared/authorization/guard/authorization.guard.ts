import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizationFacade } from '../service/authorization.facade';
import { filter, map } from 'rxjs/operators';
import { Authorization } from '../models/authorization';
import { Role } from '../enums/role';

@Injectable({ providedIn: 'root' })
export class AuthorizationGuard implements CanActivate {
	public constructor(
		private readonly authorizationFacade: AuthorizationFacade
	) {}

	public canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
		return this.authorizationFacade.getAuthorization.pipe(
			filter((auth) => !!auth),
			map((auth: Authorization) => {
				const roles = route.data.roles as Role[];
				return auth && roles.includes(auth.role);
			})
		);
	}
}
