import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import {
	selectUsers,
	selectUsersLoaded,
	selectUsersResponseMessage,
	selectUserByEmail as selectUserById
} from '../+state/selectors/selectors';
import {
	LoadAuthorizedUsers,
	AddAuthorizedUsers,
	RemoveAddAuthorizedUserMessage,
	EditAuthorizedUser,
	DeleteAuthorizedUser
} from '../+state/actions/actions';
import { User } from '../model/authorization';
import { AddUserRole } from '../interface/addUserRole';
import { Message } from '../interface/Message';
import { PatchUserRole } from '../interface/patchUserRole';
import { DeleteUser } from '../interface/deleteUser';

@Injectable()
export class AdminUserFacade {
	public constructor(private readonly store: Store) {}

	public get authorizedUsers(): Observable<User[]> {
		return this.store.select(selectUsers);
	}
	public get usersLoaded(): Observable<boolean> {
		return this.store.select(selectUsersLoaded);
	}
	public get responseMessage(): Observable<Message> {
		return this.store.select(selectUsersResponseMessage);
	}

	public authorizedUserById(id: string): Observable<User> {
		return this.store.select(selectUserById(id));
	}

	public loadAuthorizedUsers(): void {
		this.store.dispatch(new LoadAuthorizedUsers());
	}

	public addAuthorizedUsers(users: AddUserRole[]): void {
		this.store.dispatch(new AddAuthorizedUsers(users));
	}

	public editAuthorizedUser(user: PatchUserRole): void {
		this.store.dispatch(new EditAuthorizedUser(user));
	}

	public deleteAuthorizedUser(user: DeleteUser): void {
		this.store.dispatch(new DeleteAuthorizedUser(user));
	}

	public removeMessage(): void {
		this.store.dispatch(new RemoveAddAuthorizedUserMessage());
	}
}
