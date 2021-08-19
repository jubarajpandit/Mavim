import {
	createFeatureSelector,
	createSelector,
	MemoizedSelector
} from '@ngrx/store';
import { AdminUserState } from '../../interface/AdminState';
import { User } from '../../model/authorization';

const selectAdminState = createFeatureSelector<AdminUserState>('adminUsers');

export const selectUsers = createSelector(
	selectAdminState,
	(adminState) => adminState.users
);
export const selectUserByEmail = (
	userId: string
): MemoizedSelector<AdminUserState, User> =>
	createSelector(selectAdminState, (adminState) =>
		adminState.users.find((user) => user.id === userId)
	);
export const selectUsersLoaded = createSelector(
	selectAdminState,
	(adminState) => adminState.usersLoaded
);
export const selectUsersResponseMessage = createSelector(
	selectAdminState,
	(adminState) => adminState.message
);
