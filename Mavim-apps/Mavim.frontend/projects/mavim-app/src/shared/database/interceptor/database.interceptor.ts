/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import {
	HttpRequest,
	HttpHandler,
	HttpEvent,
	HttpInterceptor
} from '@angular/common/http';

import { Observable, from } from 'rxjs';
import { mergeMap, map, filter } from 'rxjs/operators';
import { DatabaseFacade } from '../service/database.facade';
import { RegexUtils } from '../../utils';
import { DatabaseID } from '../constants';

@Injectable()
export class DatabaseInterceptor implements HttpInterceptor {
	public constructor(private readonly databaseFacade: DatabaseFacade) {}

	public intercept(
		req: HttpRequest<any>,
		next: HttpHandler
	): Observable<HttpEvent<any>> {
		const scopes = req.url.includes(DatabaseID);

		// If there are no scopes set for this request, do nothing.
		if (!scopes) {
			return next.handle(req);
		}

		// Acquire a databaseID for this request, and replace the url with the selected databaseId.
		return from(
			this.databaseFacade.selectedDatabase.pipe(
				filter((dbid) => !!dbid),
				map((dbid) => {
					let newRequest = req.clone();
					if (RegexUtils.Guid().test(dbid)) {
						const url = newRequest.url.replace(DatabaseID, dbid);
						const httpRequest = new HttpRequest(
							req.method,
							url,
							{ ...req.body },
							{ headers: req.headers }
						);
						newRequest = Object.assign(newRequest, httpRequest);
					}
					return newRequest;
				}),
				mergeMap((nextReq) => next.handle(nextReq))
			)
		);
	}
}
