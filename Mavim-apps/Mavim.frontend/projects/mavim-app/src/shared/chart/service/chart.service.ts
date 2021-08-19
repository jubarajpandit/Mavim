import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { take, catchError, map } from 'rxjs/operators';
import { UrlUtils } from '../../utils';
import { ErrorService } from '../../notification/services/error.service';
import { TopicPathPrefix } from '../../../environments/constants';
import { TopicCharts } from '../models/topicCharts';
import { Chart } from '../models/chart';

@Injectable({ providedIn: 'root' })
export class ChartService {
	public constructor(
		private readonly httpClient: HttpClient,
		private readonly errorService: ErrorService
	) {}

	private readonly chartApiUrl = `${TopicPathPrefix}/topic/{dcvid}/charts`;

	public getCharts(dcvId: string): Observable<TopicCharts> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.chartApiUrl, {
			dcvId
		});
		return this.httpClient.get<Chart[]>(apiUrl, {}).pipe(
			take(1),
			map((charts) => {
				return { topicDcv: dcvId, charts } as TopicCharts;
			}),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'Charts')
			)
		);
	}
}
