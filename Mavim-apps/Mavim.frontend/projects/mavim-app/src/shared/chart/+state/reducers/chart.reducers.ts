import { ChartState } from '../../interfaces/chart-state.interface';
import { ChartActions, ChartActionTypes } from '../actions/chart.actions';
import { FetchStatus } from '../../../enums/FetchState';
import { EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { TopicCharts } from '../../models/topicCharts';
import { RootState } from '../../../../app/+state';

export interface FeatureState extends RootState {
	charts: ChartState;
}

export const chartsAdapter: EntityAdapter<TopicCharts> =
	createEntityAdapter<TopicCharts>({
		selectId: (topicChart) => topicChart.topicDcv
	});

export const initialChartState: ChartState = chartsAdapter.getInitialState({
	fetchChart: FetchStatus.NotFetched
});

export function chartReducer(
	state = initialChartState,
	action: ChartActions
): ChartState {
	switch (action.type) {
		case ChartActionTypes.LoadCharts:
			return { ...state, fetchChart: FetchStatus.Loading };
		case ChartActionTypes.LoadChartsFailed:
			return { ...state, fetchChart: FetchStatus.Fetched };
		case ChartActionTypes.LoadChartsSuccess: {
			return chartsAdapter.addOne(action.payload, {
				...state,
				fetchChart: FetchStatus.Fetched
			});
		}
		default:
			return state;
	}
}
