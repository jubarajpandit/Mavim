import { createFeatureSelector, createSelector } from '@ngrx/store';
import { RootState } from '../../../app/+state';
import { NotificationState } from '../interfaces/notification-state.interface';

// Extends the app state to include the notification feature.
// This is required because notifications are lazy loaded.
// So the reference to NotificationState cannot be added to app.state.ts directly.
export interface State extends RootState {
	notifications: NotificationState;
}

// Selector functions
const getNotificationFeatureState =
	createFeatureSelector<NotificationState>('notifications');

export const getNotifications = createSelector(
	getNotificationFeatureState,
	(state) => state.notifications
);
