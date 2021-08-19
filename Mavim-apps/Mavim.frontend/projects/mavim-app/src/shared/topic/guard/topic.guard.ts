import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { TopicFacade } from '../services/topic.facade';
import { Observable } from 'rxjs';
import { filter, map, tap } from 'rxjs/operators';
import { Topic } from '../models/topic.model';

@Injectable({ providedIn: 'root' })
export class TopicGuard implements CanActivate {
	public constructor(private readonly topicFacade: TopicFacade) {}

	public canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
		const DCVID = 'dcvid';
		const dcvId = route.params[DCVID] as string;
		// Block the route (return false), if the property does not exist or if readonly is true.
		return this.topicFacade.getTopicByDcv(dcvId).pipe(
			tap((data) =>
				!data ? this.topicFacade.loadTopicByDcv(dcvId) : data
			),
			filter((data) => !!data),
			map((data: Topic) => !data.business?.isReadOnly)
		);
	}
}
