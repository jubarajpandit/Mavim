import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { Database } from '../models/database';
import { map } from 'rxjs/operators';
import { UrlUtils } from '../../utils';
import { DatabaseApiModel } from '../models/databaseApiModel';

@Injectable({ providedIn: 'root' })
export class DatabaseService {
	public constructor(private readonly httpClient: HttpClient) {}

	private readonly databaseApiUrl = '/catalog/v1/mavimdatabases';

	public getDatabases(): Observable<Database[]> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.databaseApiUrl, {});

		return this.httpClient.get<DatabaseApiModel[]>(apiUrl).pipe(
			map((database) => {
				return database.map(
					(d) =>
						({ name: d.displayName, id: d.databaseID } as Database)
				);
			})
		);
	}
}
