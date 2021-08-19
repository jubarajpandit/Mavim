import { GenericTreeFacade } from '../../../../../shared/tree/services/generictree.facade';
import { namespace } from '../+state';
import { Store } from '@ngrx/store';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class RelationTreeFacade extends GenericTreeFacade {
	public constructor(protected readonly store: Store) {
		super(store, namespace);
	}
}
