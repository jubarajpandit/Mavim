import { Actions } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { namespace } from '../index';
import { TreeService } from '../../../../../../shared/tree/services/tree.service';
import { TreeEffectBase } from '../../../../../../shared/tree/+state/generictree.effects';

@Injectable()
export class TreeEffect extends TreeEffectBase {
	public constructor(
		protected readonly actions$: Actions,
		protected readonly treeService: TreeService
	) {
		super(actions$, treeService, namespace);
	}
}
