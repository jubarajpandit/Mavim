import { ApiWopiActionUrls } from './../models/api/api-wopi-actionurls.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { UrlUtils } from '../../utils';
import { ErrorService } from '../../notification/services/error.service';
import { Wopi } from '../models/wopi.model';
import { ApiWopi } from '../models/api/api-wopi.model';
import { WopiActionUrls } from '../models/wopi-actionurls.model';
import { WopiPathPrefix } from '../../../environments/constants';

@Injectable({
	providedIn: 'root'
})
export class WopiService {
	public constructor(
		private readonly httpClient: HttpClient,
		private readonly errorService: ErrorService
	) {}

	private readonly wopiSrc: string =
		environment.wopiSrc +
		`${WopiPathPrefix}/description/wopi/files/{dcvId}`;

	public getWopiActionUrls(): Observable<WopiActionUrls> {
		const apiUrl = UrlUtils.getUrlByAbsoluteApiUrl(
			environment.wopiSrc + '/v1/wopiactionUrl'
		);

		return this.httpClient.get<ApiWopiActionUrls>(apiUrl).pipe(
			map((actionUrls) => this.MapToWopiActionUrls(actionUrls)),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'wopiactionurls')
			)
		);
	}

	public getFileInfo(dcv: string): Observable<Wopi> {
		const apiUrl = UrlUtils.getUrlByAbsoluteApiUrl(this.wopiSrc, {
			dcvId: dcv
		});

		return this.httpClient.get<ApiWopi>(apiUrl).pipe(
			map((wopi) => this.MapToWopi(wopi, dcv)),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'Wopi')
			)
		);
	}

	private MapToWopi(wopi: ApiWopi, dcv: string): Wopi {
		return {
			topicDCV: dcv,
			hasDescription: wopi.Size > 0,
			descriptionLoaded: true
		};
	}

	private MapToWopiActionUrls(
		apiWopiActionUrl: ApiWopiActionUrls
	): WopiActionUrls {
		return {
			actionUrlId: '0',
			wopiTestViewerActionUrl: apiWopiActionUrl.WopiTestViewerActionUrl,
			wordViewerActionUrl: apiWopiActionUrl.WordViewerActionUrl,
			wordEditorActionUrl: apiWopiActionUrl.WordEditorActionUrl,
			wordNewEditorActionUrl: apiWopiActionUrl?.WordNewEditorActionUrl,
			visioViewerActionUrl: apiWopiActionUrl.VisioViewerActionUrl
		};
	}
}
