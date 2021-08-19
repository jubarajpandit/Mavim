import { Injectable } from '@angular/core';
import { Action, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import {
	featurefagRequest,
	selectFeatureflag,
	selectFeatureflags,
	selectFetchFeatureflag
} from '../+state';
import { FetchStatus } from '../../enums/FetchState';

@Injectable({ providedIn: 'root' })
export class FeatureflagFacade {
	public constructor(private readonly store: Store) {}

	public get getFeatureflags(): Observable<string[]> {
		return this.store.select(selectFeatureflags);
	}

	public get fetchFeatureflags(): Observable<FetchStatus> {
		return this.store.select(selectFetchFeatureflag);
	}

	public getFeatureflag(featureflag: string): Observable<boolean> {
		return this.store.select(selectFeatureflag(featureflag));
	}

	public loadFeatureflags(): void {
		this.store.dispatch(featurefagRequest() as Action);
	}
}
