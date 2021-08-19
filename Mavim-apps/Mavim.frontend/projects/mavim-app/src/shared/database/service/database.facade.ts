import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Database } from '../models/database';
import { Observable } from 'rxjs';
import * as actions from '../+state/actions/database.actions';
import {
	selectDatabases,
	selectSelectedDatabases,
	selectFetchDatabase
} from '../+state/selectors/database.selector';
import { FetchStatus } from '../../enums/FetchState';

@Injectable({ providedIn: 'root' })
export class DatabaseFacade {
	public constructor(private readonly store: Store) {}

	public get database(): Observable<Database[]> {
		return this.store.select(selectDatabases);
	}
	public get selectedDatabase(): Observable<string> {
		return this.store.select(selectSelectedDatabases);
	}
	public get fetchDatabase(): Observable<FetchStatus> {
		return this.store.select(selectFetchDatabase);
	}

	public initDatabase(): void {
		this.store.dispatch(new actions.InitDatabase());
	}

	public loadDatabase(): void {
		this.store.dispatch(new actions.LoadDatabase());
	}
}
