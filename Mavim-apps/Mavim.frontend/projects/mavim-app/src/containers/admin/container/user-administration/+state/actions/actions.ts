import { Action } from '@ngrx/store';
import { User } from '../../model/authorization';
import { AddUserRole } from '../../interface/addUserRole';
import { PatchUserRole } from '../../interface/patchUserRole';
import { DeleteUser } from '../../interface/deleteUser';

/**
 * For each action type in an action group, make a simple
 * enum object for all of this group's action types.
 */
export enum AdminActionTypes {
	LoadAuthorizedUsers = '[Admin User] Load Authorized Users',
	SuccessLoadAuthorizedUsers = '[Admin User] Success Load Authorized Users',
	FailedLoadAuthorizedUsers = '[Admin User] Failed Load Authorized Users',
	AddAuthorizedUsers = '[Admin User] Add Authorized Users',
	SuccessAddAuthorizedUsers = '[Admin User] Success Add Authorized Users',
	FailedAddAuthorizedUsers = '[Admin User] Failed Add Authorized Users',
	EditAuthorizedUser = '[Admin User] Edit Authorized User',
	SuccessEditAuthorizedUser = '[Admin User] Success Edit Authorized User',
	FailedEditAuthorizedUser = '[Admin User] Failed Edit Authorized User',
	DeleteAuthorizedUser = '[Admin User] Delete Authorized User',
	SuccessDeleteAuthorizedUser = '[Admin User] Success Delete Authorized User',
	FailedDeleteAuthorizedUser = '[Admin User] Failed Delete Authorized User',
	RemoveAddAuthorizedUserMessage = '[Admin User] Remove Authorized Users Message'
}

/**
 * Every action is comprised of at least a type and an optional
 * payload. Expressing actions as classes enables powerful
 * type checking in reducer functions.
 */
export class LoadAuthorizedUsers implements Action {
	public readonly type = AdminActionTypes.LoadAuthorizedUsers;
}

export class SuccessLoadAuthorizedUsers implements Action {
	public constructor(public payload: User[]) {}
	public readonly type = AdminActionTypes.SuccessLoadAuthorizedUsers;
}

export class FailedLoadAuthorizedUsers implements Action {
	public readonly type = AdminActionTypes.FailedLoadAuthorizedUsers;
}

export class AddAuthorizedUsers implements Action {
	public constructor(public payload: AddUserRole[]) {}
	public readonly type = AdminActionTypes.AddAuthorizedUsers;
}

export class SuccessAddAuthorizedUsers implements Action {
	public constructor(public payload: User[]) {}
	public readonly type = AdminActionTypes.SuccessAddAuthorizedUsers;
}

export class FailedAddAuthorizedUsers implements Action {
	public constructor(public payload: string) {}
	public readonly type = AdminActionTypes.FailedAddAuthorizedUsers;
}

export class EditAuthorizedUser implements Action {
	public constructor(public payload: PatchUserRole) {}
	public readonly type = AdminActionTypes.EditAuthorizedUser;
}

export class SuccessEditAuthorizedUser implements Action {
	public constructor(public payload: PatchUserRole) {}
	public readonly type = AdminActionTypes.SuccessEditAuthorizedUser;
}

export class FailedEditAuthorizedUser implements Action {
	public constructor(public payload: string) {}
	public readonly type = AdminActionTypes.FailedEditAuthorizedUser;
}

export class DeleteAuthorizedUser implements Action {
	public constructor(public payload: DeleteUser) {}
	public readonly type = AdminActionTypes.DeleteAuthorizedUser;
}

export class SuccessDeleteAuthorizedUser implements Action {
	public constructor(public payload: DeleteUser) {}
	public readonly type = AdminActionTypes.SuccessDeleteAuthorizedUser;
}

export class FailedDeleteAuthorizedUser implements Action {
	public constructor(public payload: string) {}
	public readonly type = AdminActionTypes.FailedDeleteAuthorizedUser;
}

export class RemoveAddAuthorizedUserMessage implements Action {
	public readonly type = AdminActionTypes.RemoveAddAuthorizedUserMessage;
}

/**
 * Export a type alias of all actions in this action group
 * so that reducers can easily compose action types
 */
export type AdminActions =
	| LoadAuthorizedUsers
	| SuccessLoadAuthorizedUsers
	| FailedLoadAuthorizedUsers
	| AddAuthorizedUsers
	| SuccessAddAuthorizedUsers
	| FailedAddAuthorizedUsers
	| EditAuthorizedUser
	| SuccessEditAuthorizedUser
	| FailedEditAuthorizedUser
	| DeleteAuthorizedUser
	| SuccessDeleteAuthorizedUser
	| FailedDeleteAuthorizedUser
	| RemoveAddAuthorizedUserMessage;
