import { Action } from '@ngrx/store';
import { Authorization } from '../../models/authorization';

export enum AuthorizationActionTypes {
	LoadAuthorization = '[Authorization] Load Authorization',
	LoadAuthorizationSuccess = '[Authorization] Load Authorization Success',
	LoadAuthorizationFailed = '[Authorization] Load Authorization Failed'
}

// Action Creators
export class LoadAuthorization implements Action {
	public readonly type = AuthorizationActionTypes.LoadAuthorization;
}
export class LoadAuthorizationSuccess implements Action {
	public constructor(public authorization: Authorization) {}
	public readonly type = AuthorizationActionTypes.LoadAuthorizationSuccess;
}
export class LoadAuthorizationFailed implements Action {
	public readonly type = AuthorizationActionTypes.LoadAuthorizationFailed;
}

export type AuthorizationActions =
	| LoadAuthorization
	| LoadAuthorizationSuccess
	| LoadAuthorizationFailed;
