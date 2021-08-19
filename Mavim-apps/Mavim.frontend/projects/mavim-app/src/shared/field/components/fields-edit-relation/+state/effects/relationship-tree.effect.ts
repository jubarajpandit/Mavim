import { Actions } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { TreeService } from '../../../../../../shared/tree/services/tree.service';
import { namespace } from '../index';
import { TreeEffectBase } from '../../../../../tree/+state/generictree.effects';

@Injectable()
export class RelationshipTreeEffect extends TreeEffectBase {
	public constructor(
		protected readonly actions$: Actions,
		protected readonly treeService: TreeService
	) {
		super(actions$, treeService, namespace);
	}
}
