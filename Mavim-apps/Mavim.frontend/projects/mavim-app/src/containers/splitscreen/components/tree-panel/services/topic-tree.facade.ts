import { GenericTreeFacade } from '../../../../../shared/tree/services/generictree.facade';
import { Store } from '@ngrx/store';
import { Injectable } from '@angular/core';
import { namespace } from '../+state';

@Injectable({ providedIn: 'root' })
export class TopicTreeFacade extends GenericTreeFacade {
	public constructor(protected readonly store: Store) {
		super(store, namespace);
	}
}
