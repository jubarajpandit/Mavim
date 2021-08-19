import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';
import { UrlUtils } from '../../../../../shared/utils';
import { User } from '../model/authorization';
import { AddUserRole } from '../interface/addUserRole';
import { PatchUserRole } from '../interface/patchUserRole';
import { DeleteUser } from '../interface/deleteUser';

const httpOptions = {
	headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })
export class AdminAuthorizationService {
	public constructor(private readonly httpClient: HttpClient) {}

	private readonly authorizationUsersApiUrl = '/authorization/v1/authorize';
	private readonly authorizationUserApiUrl =
		'/authorization/v1/authorize/user/{id}';

	public getAuthorizationUsers(): Observable<User[]> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.authorizationUsersApiUrl,
			{}
		);

		return this.httpClient.get<User[]>(apiUrl, httpOptions);
	}

	public addAuthorizationUsers(users: AddUserRole[]): Observable<User[]> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.authorizationUsersApiUrl,
			{}
		);

		return this.httpClient.post<User[]>(
			apiUrl,
			users.map((u) => this.map(u)),
			httpOptions
		);
	}

	public patchAuthorizationUsers(user: PatchUserRole): Observable<void> {
		const { id, role } = user;
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.authorizationUserApiUrl,
			{ id }
		);

		return this.httpClient.patch<void>(apiUrl, { role }, httpOptions);
	}

	public deleteAuthorizationUsers(user: DeleteUser): Observable<void> {
		const { id } = user;
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.authorizationUserApiUrl,
			{ id }
		);

		return this.httpClient.delete<void>(apiUrl, httpOptions);
	}

	private map(user: AddUserRole): User {
		return {
			email: user.email,
			role: user.role
		} as User;
	}
}
