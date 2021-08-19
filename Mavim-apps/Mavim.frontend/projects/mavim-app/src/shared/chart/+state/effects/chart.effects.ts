import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { Observable, of } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';
import * as chartActions from '../actions/chart.actions';
import { ChartService } from '../../service/chart.service';
import { Action } from '@ngrx/store';

@Injectable()
export class ChartEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly chartService: ChartService
	) {}

	public getTopicCharts$: Observable<Action> = createEffect(() => {
		return this.actions$.pipe(
			ofType(chartActions.ChartActionTypes.LoadCharts),
			map((action: chartActions.LoadCharts) => action.payload),
			mergeMap((dcvId) =>
				this.chartService.getCharts(dcvId).pipe(
					map((topicCharts) => {
						return new chartActions.LoadChartSuccess(topicCharts);
					}),
					catchError(() => of(new chartActions.LoadChartFailed()))
				)
			)
		);
	});
}
