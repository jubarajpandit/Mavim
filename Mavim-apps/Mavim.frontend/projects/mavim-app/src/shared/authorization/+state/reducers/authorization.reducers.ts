import { AuthorizationState } from '../../interfaces/authorization-state.interface';
import {
	AuthorizationActions,
	AuthorizationActionTypes
} from '../actions/authentication.actions';

export const initialAuthorizationState: AuthorizationState = {
	account: undefined
};

export function authorizationReducer(
	state = initialAuthorizationState,
	action: AuthorizationActions
): AuthorizationState {
	switch (action.type) {
		case AuthorizationActionTypes.LoadAuthorizationSuccess:
			return { ...state, account: action.authorization };

		default:
			return state;
	}
}
