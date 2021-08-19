import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import { UrlUtils } from '../../utils';
import { ErrorService } from '../../notification/services/error.service';
import { TopicPathPrefix } from '../../../environments/constants';
import { Dictionary } from '@ngrx/entity';
import { TopicMetaType } from '../models/topic-meta-type.model';

const headers = new HttpHeaders().set('Content-Type', 'application/json');

@Injectable({
	providedIn: 'root'
})
export class TopicMetaService {
	public constructor(
		private readonly httpClient: HttpClient,
		private readonly errorService: ErrorService
	) {}

	private readonly topicMetaTypesApiUrl = `${TopicPathPrefix}/topic/{topicId}/types`;
	private readonly topicMetaIconsApiUrl = `${TopicPathPrefix}/types/{elementType}/Icons`;

	public getTopicTypes(
		topicId: string
	): Observable<Dictionary<TopicMetaType>> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicMetaTypesApiUrl,
			{ topicId }
		);
		return this.httpClient
			.get<Dictionary<TopicMetaType>>(apiUrl, { headers })
			.pipe(
				take(1),
				catchError((err) =>
					this.errorService.handleServiceError(err, 'Topic Types')
				)
			);
	}

	public getTopicIcons(elementType: string): Observable<Dictionary<string>> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicMetaIconsApiUrl,
			{ elementType }
		);
		return this.httpClient
			.get<Dictionary<string>>(apiUrl, { headers })
			.pipe(
				take(1),
				catchError((err) =>
					this.errorService.handleServiceError(err, 'Topic Icons')
				)
			);
	}
}
