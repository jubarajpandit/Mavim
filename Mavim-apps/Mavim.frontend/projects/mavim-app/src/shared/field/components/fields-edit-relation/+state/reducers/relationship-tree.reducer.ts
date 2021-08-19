import { TreeState } from '../../../../../../shared/tree/interfaces/tree-state.interface';
import { namespace } from '..';
import {
	createTreeReducerWithNameData,
	topicTreeReducer
} from '../../../../../tree/+state/generictree.reducer';
import { Action } from '@ngrx/store';
import { ActionPayloadWithNameSpace } from '../../../../../tree/interfaces/action-payload-with-namespace';

export function relationshipTreeReducer(
	state: TreeState,
	action: Action
): TreeState {
	return 'payload' in action &&
		action['payload'] &&
		typeof action['payload'] === 'object' &&
		'namespace' in action['payload']
		? createTreeReducerWithNameData(namespace)(
				state,
				action as ActionPayloadWithNameSpace
		  )
		: topicTreeReducer(state, action);
}
