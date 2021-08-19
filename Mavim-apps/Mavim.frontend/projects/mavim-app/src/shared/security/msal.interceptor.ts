/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import {
	HttpRequest,
	HttpHandler,
	HttpEvent,
	HttpInterceptor,
	HttpErrorResponse
} from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { mergeMap, tap } from 'rxjs/operators';
import { AuthResponse } from 'msal';
import { MsalService, BroadcastService } from '@azure/msal-angular';

@Injectable()
export class MsalInterceptor implements HttpInterceptor {
	public constructor(
		private readonly auth: MsalService,
		private readonly broadcastService: BroadcastService
	) {}

	private static readonly UnAuthorizedStatusCode = 401;

	public intercept(
		req: HttpRequest<any>,
		next: HttpHandler
	): Observable<HttpEvent<any>> {
		const scopes = this.auth.getScopesForEndpoint(req.url);

		// If there are no scopes set for this request, do nothing.
		if (!scopes) {
			return next.handle(req);
		}

		let token: string;

		// Acquire a token for this request, and attach as proper auth header.
		return from(
			this.auth
				.acquireTokenSilent({ scopes })
				.then((response: AuthResponse) => {
					token = this.getTokenFromResponse(response);
					const authHeader = `Bearer ${token}`;

					const newRequest = req.clone({
						setHeaders: {
							Authorization: authHeader
						}
					});

					return newRequest;
				})
		).pipe(
			mergeMap((nextReq: HttpRequest<any>) => next.handle(nextReq)),
			tap(
				// eslint-disable-next-line @typescript-eslint/no-empty-function
				() => {},
				(err) => {
					if (
						err instanceof HttpErrorResponse &&
						err.status === MsalInterceptor.UnAuthorizedStatusCode
					) {
						this.auth.clearCacheForScope(token);
						this.broadcastService.broadcast(
							'msal:notAuthorized',
							err.message
						);
					}
				}
			)
		);
	}

	private getTokenFromResponse(response: AuthResponse): string {
		return response.idToken.rawIdToken;
	}
}
