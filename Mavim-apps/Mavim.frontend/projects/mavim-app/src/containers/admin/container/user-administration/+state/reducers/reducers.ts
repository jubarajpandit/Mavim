import { AdminActions, AdminActionTypes } from '../actions/actions';
import { AdminUserState } from '../../interface/AdminState';
import { MessageType } from '../../enums/MessageType';
import { User } from '../../model/authorization';

const initialState: AdminUserState = {
	users: [],
	usersLoaded: false,
	message: undefined
};

export function reducer(
	state = initialState,
	action: AdminActions
): AdminUserState {
	switch (action.type) {
		case AdminActionTypes.LoadAuthorizedUsers: {
			return { ...state, usersLoaded: false };
		}
		case AdminActionTypes.SuccessLoadAuthorizedUsers: {
			return {
				...state,
				users: orderByEmail(action.payload),
				usersLoaded: true
			};
		}
		case AdminActionTypes.SuccessAddAuthorizedUsers: {
			const { users } = state;
			const { payload: newUsers } = action;
			return {
				...state,
				users: orderByEmail([...users, ...newUsers]),
				message: { text: addSuccessMessage, type: MessageType.Success }
			};
		}
		case AdminActionTypes.SuccessEditAuthorizedUser: {
			const { users } = state;
			const { payload } = action;
			const newUsers = users.map((u) => {
				if (u.id === payload.id) {
					return { ...u, role: payload.role };
				}
				return u;
			});

			return {
				...state,
				users: newUsers,
				message: { text: editSuccessMessage, type: MessageType.Success }
			};
		}
		case AdminActionTypes.SuccessDeleteAuthorizedUser: {
			const { users } = state;
			const { payload } = action;
			const newUsers = users.filter((u) => u.id !== payload.id);
			return {
				...state,
				users: newUsers,
				message: {
					text: deleteSuccessMessage,
					type: MessageType.Success
				}
			};
		}
		case AdminActionTypes.FailedAddAuthorizedUsers:
		case AdminActionTypes.FailedEditAuthorizedUser:
		case AdminActionTypes.FailedDeleteAuthorizedUser: {
			return {
				...state,
				message: { text: action.payload, type: MessageType.Failed }
			};
		}
		case AdminActionTypes.RemoveAddAuthorizedUserMessage: {
			return { ...state, message: undefined };
		}
		default: {
			return state;
		}
	}
}

const orderByEmail = (users: User[]): User[] =>
	users
		.map((u) => ({ ...u }))
		.sort((a, b) => (a.email > b.email ? 1 : b.email > a.email ? -1 : 0));

const addSuccessMessage = 'User has successfully been added to the list.';
const editSuccessMessage = 'User role has been successfully updated.';
const deleteSuccessMessage = 'User is successfully removed.';
