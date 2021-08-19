import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { Authorization } from '../models/authorization';
import { UrlUtils } from '../../utils';

@Injectable({ providedIn: 'root' })
export class AuthorizationService {
	public constructor(private readonly httpClient: HttpClient) {}

	private readonly authorizationApiUrl = '/authorization/v1/authorize/user';

	public getAuthorizationRights(): Observable<Authorization> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.authorizationApiUrl,
			{}
		);

		return this.httpClient.get<Authorization>(apiUrl);
	}
}
