import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import * as actions from '../+state/actions/actions';
import {
	selectGroupFetchStatus,
	selectGroups
} from '../+state/selectors/selector';
import { FetchStatus } from '../../../../../shared/enums/FetchState';
import { Group } from '../model/group';

@Injectable({ providedIn: 'root' })
export class GroupFacade {
	public constructor(private readonly store: Store) {}

	public get getGroupFetchStatus(): Observable<FetchStatus> {
		return this.store.select(selectGroupFetchStatus);
	}

	public get getGroups(): Observable<Group[]> {
		return this.store.select(selectGroups);
	}

	public loadGroups(): void {
		this.store.dispatch(actions.loadGroups());
	}
}
