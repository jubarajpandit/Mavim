import { createReducer, on } from '@ngrx/store';
import { RouterState } from '.';
import * as routingActions from './routing.actions';

const initialRouterState: RouterState = {
	queue: []
};

export const reducer = createReducer(
	initialRouterState,
	on(
		routingActions.Init,
		routingActions.UpdateQueue,
		(state, { payload }): RouterState => ({ ...state, queue: payload })
	)
);
