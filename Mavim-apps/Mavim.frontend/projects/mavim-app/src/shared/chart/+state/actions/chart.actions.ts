import { Action } from '@ngrx/store';
import { TopicCharts } from '../../models/topicCharts';

export enum ChartActionTypes {
	LoadCharts = '[Chart] Load Charts',
	LoadChartsSuccess = '[Chart] Load Charts Success',
	LoadChartsFailed = '[Chart] Load Charts Failed'
}

// Action Creators
export class LoadCharts implements Action {
	public constructor(public payload: string) {}
	public readonly type = ChartActionTypes.LoadCharts;
}
export class LoadChartSuccess implements Action {
	public constructor(public payload: TopicCharts) {}
	public readonly type = ChartActionTypes.LoadChartsSuccess;
}
export class LoadChartFailed implements Action {
	public readonly type = ChartActionTypes.LoadChartsFailed;
}

export type ChartActions = LoadCharts | LoadChartSuccess | LoadChartFailed;
