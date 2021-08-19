import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import * as actions from '../+state/actions/chart.actions';
import {
	selectChartsById,
	selectFetchChart
} from '../+state/selectors/chart.selector';
import { FetchStatus } from '../../enums/FetchState';
import { TopicCharts } from '../models/topicCharts';

@Injectable({ providedIn: 'root' })
export class ChartFacade {
	public constructor(private readonly store: Store) {}

	public getCharts(dcvId: string): Observable<TopicCharts> {
		return this.store.select(selectChartsById(dcvId));
	}
	public get fetchCharts(): Observable<FetchStatus> {
		return this.store.select(selectFetchChart);
	}

	public loadCharts(dcvId: string): void {
		this.store.dispatch(new actions.LoadCharts(dcvId));
	}
}
