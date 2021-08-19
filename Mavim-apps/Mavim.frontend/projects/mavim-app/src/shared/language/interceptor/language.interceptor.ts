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
import { Language } from '../constants';
import { LanguageFacade } from '../service/language.facade';

@Injectable()
export class LanguageInterceptor implements HttpInterceptor {
	public constructor(private readonly languageFacade: LanguageFacade) {}

	public intercept(
		req: HttpRequest<any>,
		next: HttpHandler
	): Observable<HttpEvent<any>> {
		const scopes = req.url.includes(Language);

		// If there are no scopes set for this request, do nothing.
		if (!scopes) {
			return next.handle(req);
		}

		// Acquire a Language for this request, and replace the url with the selected language.
		return from(
			this.languageFacade.language.pipe(
				filter((language) => !!language),
				map((language) => {
					let newRequest = req.clone();
					const url = newRequest.url.replace(Language, language);
					const httpRequest = new HttpRequest(
						req.method,
						url,
						{ ...req.body },
						{ headers: req.headers }
					);
					newRequest = Object.assign(newRequest, httpRequest);
					return newRequest;
				}),
				mergeMap((nextReq) => next.handle(nextReq))
			)
		);
	}
}
