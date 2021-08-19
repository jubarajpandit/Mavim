import { FetchStatus } from '../../enums/FetchState';
import { EntityState } from '@ngrx/entity';
import { TopicCharts } from '../models/topicCharts';

export interface ChartState extends EntityState<TopicCharts> {
	fetchChart: FetchStatus;
}
