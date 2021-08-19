import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import * as actions from '../+state/actions/authentication.actions';
import { Observable } from 'rxjs';
import { selectEditTopic } from '../+state/selectors/authorization.selector';
import { Authorization } from '../models/authorization';

@Injectable({ providedIn: 'root' })
export class AuthorizationFacade {
	public constructor(private readonly store: Store) {}

	public get getAuthorization(): Observable<Authorization> {
		return this.store.select(selectEditTopic);
	}

	public loadAuthorization(): void {
		this.store.dispatch(new actions.LoadAuthorization());
	}
}
