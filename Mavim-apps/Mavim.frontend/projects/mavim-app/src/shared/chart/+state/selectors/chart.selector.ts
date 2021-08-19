import {
	createFeatureSelector,
	createSelector,
	DefaultProjectorFn,
	MemoizedSelector
} from '@ngrx/store';
import { ChartState } from '../../interfaces/chart-state.interface';
import { TopicCharts } from '../../models/topicCharts';

export const selectChartState = createFeatureSelector<ChartState>('charts');

export const selectChartsById = (
	dcv: string
): MemoizedSelector<ChartState, TopicCharts, DefaultProjectorFn<TopicCharts>> =>
	createSelector(selectChartState, (chartState) => chartState.entities[dcv]);

export const selectFetchChart = createSelector(
	selectChartState,
	(chartState: ChartState) => chartState?.fetchChart
);
