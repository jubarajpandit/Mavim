import { createReducer, on } from '@ngrx/store';
import { RootState } from '../../../../app/+state';
import { NotificationState } from '../../interfaces/notification-state.interface';
import * as NotificationActions from '../actions/notification.actions';

export interface FeatureState extends RootState {
	notifications: NotificationState;
}

export const initialNotificationState: NotificationState = {
	notifications: []
};

export const notificationReducer = createReducer(
	initialNotificationState,
	on(NotificationActions.AddNotification, (state, { payload }) => ({
		...state,
		notifications: state.notifications.concat([payload])
	})),
	on(NotificationActions.CloseLastNotification, (state) => ({
		...state,
		notifications: state.notifications.slice(0, -1)
	}))
);
