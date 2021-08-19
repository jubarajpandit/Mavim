import { Injectable } from '@angular/core';
import {
	HttpClient,
	HttpErrorResponse,
	HttpHeaders
} from '@angular/common/http';
import { ErrorService } from '../../notification/services/error.service';
import { Observable } from 'rxjs';
import { Path } from '../models/api/path';
import { UrlUtils } from '../../utils';
import { catchError, take } from 'rxjs/operators';
import { TopicPathPrefix } from '../../../environments/constants';

const headers = new HttpHeaders().set('Content-Type', 'application/json');

@Injectable({
	providedIn: 'root'
})
export class TreeService {
	public constructor(
		private readonly httpClient: HttpClient,
		private readonly errorService: ErrorService
	) {}

	private readonly topicPathApiUrl = `${TopicPathPrefix}/path/{dcvid}`;
	private readonly topicMoveToTopUrl = `${TopicPathPrefix}/topic/{topicId}/movetotop`;
	private readonly topicMoveToBottomUrl = `${TopicPathPrefix}/topic/{topicId}/movetobottom`;
	private readonly topicMoveUpUrl = `${TopicPathPrefix}/topic/{topicId}/moveup`;
	private readonly topicMoveDownUrl = `${TopicPathPrefix}/topic/{topicId}/movedown`;
	private readonly topicMoveLevelUpUrl = `${TopicPathPrefix}/topic/{topicId}/movelevelup`;
	private readonly topicMoveLevelDownUrl = `${TopicPathPrefix}/topic/{topicId}/moveleveldown`;

	public getPathToTopic(dcvId: string): Observable<Path> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicPathApiUrl, {
			dcvId
		});
		return this.httpClient.get<Path>(apiUrl, { headers }).pipe(
			take(1),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'Tree Get Path')
			)
		);
	}

	public moveToTop(topicId: string): Observable<void> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicMoveToTopUrl, {
			topicId
		});
		return this.httpClient
			.patch<void>(apiUrl, { headers })
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(err, 'moveToTop')
				)
			);
	}

	public moveToBottom(topicId: string): Observable<void> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicMoveToBottomUrl,
			{ topicId }
		);
		return this.httpClient
			.patch<void>(apiUrl, { headers })
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(err, 'moveToBottom')
				)
			);
	}

	public moveUp(topicId: string): Observable<void> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicMoveUpUrl, {
			topicId
		});
		return this.httpClient
			.patch<void>(apiUrl, { headers })
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(err, 'moveUp')
				)
			);
	}

	public moveDown(topicId: string): Observable<void> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicMoveDownUrl, {
			topicId
		});
		return this.httpClient
			.patch<void>(apiUrl, { headers })
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(err, 'moveDown')
				)
			);
	}

	public moveLevelUp(topicId: string): Observable<void> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicMoveLevelUpUrl,
			{ topicId }
		);
		return this.httpClient
			.patch<void>(apiUrl, { headers })
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(err, 'moveLevelUp')
				)
			);
	}

	public moveLevelDown(topicId: string): Observable<void> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicMoveLevelDownUrl,
			{ topicId }
		);
		return this.httpClient
			.patch<void>(apiUrl, { headers })
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(err, 'moveLevelDown')
				)
			);
	}
}
