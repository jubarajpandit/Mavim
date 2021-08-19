import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { UrlUtils } from '../../utils';

@Injectable({ providedIn: 'root' })
export class FeatureflagService {
	public constructor(private readonly httpClient: HttpClient) {}

	private readonly featureflagApiUrl = '/featureflag/v1/featureflag';

	public getAuthorizationRights(): Observable<string[]> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.featureflagApiUrl,
			{}
		);

		return this.httpClient.get<string[]>(apiUrl);
	}
}
