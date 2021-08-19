import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Relation } from '../models/relation.model';
import { catchError, map, tap, take } from 'rxjs/operators';
import { UrlUtils } from '../../utils';
import { ErrorService } from '../../notification/services/error.service';
import { PostRelation } from '../models/post-relation.model';
import { RelationshipType } from '../enums/relationship-types.enum';
import { TopicPathPrefix } from '../../../environments/constants';

@Injectable({
	providedIn: 'root'
})
export class RelationService {
	public constructor(
		private readonly httpClient: HttpClient,
		private readonly errorService: ErrorService
	) {}

	private readonly topicRelationsApiUrl = `${TopicPathPrefix}/topic/{dcvid}/relations`;
	private readonly relationUpdateApiUrl = `${TopicPathPrefix}/relation`;
	private readonly relationDeleteApiUrl = `${TopicPathPrefix}/topic/{dcvId}/relation/{relationId}`;

	public getRelations(topicId: string): Observable<Relation[]> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicRelationsApiUrl,
			{ dcvId: topicId }
		);

		return this.httpClient.get(apiUrl).pipe(
			take(1),
			map((relations: Relation[]) =>
				relations.map(
					(relation: Relation): Relation => ({
						...relation,
						topicDCV: topicId
					})
				)
			),
			catchError((err: HttpErrorResponse) => {
				return this.errorService.handleServiceError(err, 'Relations');
			})
		);
	}

	public createRelation(newRelation: Relation): Observable<Relation> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.relationUpdateApiUrl,
			{}
		);
		const postObject: PostRelation = {
			fromElementDcv: newRelation.topicDCV,
			toElementDcv: newRelation.withElement.dcv,
			relationshipType: RelationService.getEnumKeyByEnumValue(
				RelationshipType,
				newRelation.category
			)
		};

		return this.httpClient.post<Relation>(apiUrl, postObject).pipe(
			tap((relation: Relation) => {
				relation.topicDCV = newRelation.topicDCV;
			}),
			catchError((err: HttpErrorResponse) => {
				return this.errorService.handleServiceError(err, 'Relations');
			})
		);
	}

	public deleteRelation(deleteRelation: Relation): Observable<void> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.relationDeleteApiUrl,
			{
				dcvId: deleteRelation.topicDCV,
				relationId: deleteRelation.dcv
			}
		);

		return this.httpClient.delete<void>(apiUrl).pipe(
			catchError((err: HttpErrorResponse) => {
				return this.errorService.handleServiceError(err, 'Relations');
			})
		);
	}

	private static getEnumKeyByEnumValue(myEnum, enumValue): string {
		const keys = Object.keys(myEnum).filter((x) => myEnum[x] === enumValue);
		return keys.length > 0 ? keys[0] : undefined;
	}
}
