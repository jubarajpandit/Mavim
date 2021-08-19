import { Injectable } from '@angular/core';
import { Topic } from '../models/topic.model';
import {
	HttpClient,
	HttpErrorResponse,
	HttpHeaders
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import { UrlUtils } from '../../utils';
import { ErrorService } from '../../notification/services/error.service';
import { TopicPathPrefix } from '../../../environments/constants';

// todo: headers in env config.
const headers = new HttpHeaders().set('Content-Type', 'application/json');

@Injectable({
	providedIn: 'root'
})
export class TopicService {
	public constructor(
		private readonly httpClient: HttpClient,
		private readonly errorService: ErrorService
	) {}

	private readonly topicRootApiUrl = `${TopicPathPrefix}/root`;
	private readonly topicApiUrl = `${TopicPathPrefix}/topic/{topicId}`;
	private readonly topicChildrenApiUrl = `${TopicPathPrefix}/topic/{topicId}/children`;
	private readonly topicSiblingsApiUrl = `${TopicPathPrefix}/topic/{topicId}/siblings`;
	private readonly topicRelationshipsCategoriesApiUrl = `${TopicPathPrefix}/systemobjects/topic/relationshipcategories`;

	public getChildTopics(topicId: string): Observable<Topic[]> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicChildrenApiUrl,
			{ topicId }
		);
		return this.httpClient.get<Topic[]>(apiUrl, { headers }).pipe(
			take(1),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'Tree Child topics')
			)
		);
	}

	public getTopicSiblings(topicId: string): Observable<Topic[]> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicSiblingsApiUrl,
			{ topicId }
		);
		return this.httpClient.get<Topic[]>(apiUrl, { headers }).pipe(
			take(1),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'Sibling topics')
			)
		);
	}

	public getTreeRoot(): Observable<Topic> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicRootApiUrl,
			{}
		);
		return this.httpClient.get<Topic>(apiUrl, { headers }).pipe(
			take(1),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'TreeRoot')
			)
		);
	}

	public getRelationshipTypes(): Observable<Topic[]> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicRelationshipsCategoriesApiUrl,
			{}
		);
		return this.httpClient.get<Topic[]>(apiUrl, { headers }).pipe(
			take(1),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'TreeRoot')
			)
		);
	}

	public updateTopicName(topic: Topic): Observable<Topic> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicApiUrl, {
			topicId: topic.dcv
		});
		return this.httpClient
			.patch<Topic>(apiUrl, topic, { headers })
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(err, 'Topics')
				)
			);
	}

	public getTopicByDcv(topicId: string): Observable<Topic> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicApiUrl, {
			topicId
		});
		const httpNotFoundStatusCode = 404;
		return this.httpClient.get<Topic>(apiUrl, { headers }).pipe(
			take(1),
			catchError((err: HttpErrorResponse) => {
				if (err.status !== httpNotFoundStatusCode) {
					this.errorService.handleServiceError(err, 'Topic');
				}
				return throwError(err);
			})
		);
	}

	public createTopicById(
		topicId: string,
		name: string,
		type: string,
		icon: string
	): Observable<Topic> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicApiUrl, {
			topicId
		});
		return this.httpClient
			.post<Topic>(apiUrl, { name, type, icon }, { headers })
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(err, 'Create topic')
				)
			);
	}

	public createChildTopicById(
		topicId: string,
		name: string,
		type: string,
		icon: string
	): Observable<Topic> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicChildrenApiUrl,
			{ topicId }
		);
		return this.httpClient
			.post<Topic>(apiUrl, { name, type, icon }, { headers })
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(
						err,
						'Create child topic'
					)
				)
			);
	}

	public deleteTopicById(topicId: string): Observable<void> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicApiUrl, {
			topicId
		});

		return this.httpClient
			.delete<void>(apiUrl)
			.pipe(
				catchError((err: HttpErrorResponse) =>
					this.errorService.handleServiceError(err, 'Delete topic')
				)
			);
	}
}
